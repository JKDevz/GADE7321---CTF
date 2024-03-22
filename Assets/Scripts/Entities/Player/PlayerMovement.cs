using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    #region VARIABLES

    [Header("-- Movement Settings Reference")]
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Player player;

    //Input Variables
    private Vector2 moveInput;

    protected bool isWalkHeld;

    #endregion

    #region EVENTS

    public delegate void OnTrigger();
    public static event OnTrigger onTrigger;

    public delegate void OnMoving();
    public static event OnMoving onMoving;

    public delegate void OnSprinting();
    public static event OnSprinting onSprinting;

    #endregion

    #region ENABLES

    private void OnEnable()
    {
        player.onDamage += OnDamage;
        GameManager.onRoundSetup += OnRespawned;
    }

    private void OnDisable()
    {
        player.onDamage -= OnDamage;
        GameManager.onRoundSetup -= OnRespawned;
    }

    #endregion

    #region MAIN LOOP

    private void Awake()
    {
        if (TryGetComponent<Player>(out Player player))
        {
            this.player = player;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.GetGameState() == GameState.RoundPlaying)
        {
            TryMove();
        }

        LoadAnimations();
    }

    #endregion

    #region PLAYER ACTIONS

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        if (ctx.started)
        {
            isWalkHeld = true;
        }
        else if (ctx.canceled)
        {
            isWalkHeld = false;
        }
    }

    #endregion

    #region METHODS

    public void TryMove()
    {
        if (moveInput.magnitude != 0)//IF the player is moving
        {
            Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);
            float moveSpeed = player.playerStats.speed + player.playerStats.speedModifier;

            if (player.Inventory.HasFlag() == true)
            {
                moveSpeed -= (moveSpeed * player.playerStats.flagCarryModifier);
            }

            if (player.playerStats.isStunned == true)
            {
                moveSpeed /= 2;
            }

            agent.velocity = move * moveSpeed;

        }
    }

    private void OnDamage(GameObject attacker)
    {
        if (!player.playerStats.isStunned)//IF the player is not stunned, stun them
        {
            StartCoroutine(Stunned());
        }
        else//IF the player is already stunned, reset the stun duration
        {
            player.playerStats.stunTimer = Time.time + player.playerSettings.stunDuration;
        }

        //IF the player is holding their flag, drop it
        if (player.Inventory.HasFlag())
        {
            player.Inventory.DropFlag();
        }

        StopCoroutine(Knockback(attacker));
        StartCoroutine(Knockback(attacker));
    }

    private void OnRespawned()
    {
        StopCoroutine(Knockback(null));
        StopCoroutine(Stunned());
        player.playerStats.isStunned = false;
        player.Inventory.ClearItem();
    }

    private IEnumerator Stunned()
    {
        player.playerStats.stunTimer = Time.time + player.playerSettings.stunDuration;
        player.playerStats.isStunned = true;
        yield return new WaitUntil(() => Time.time > player.playerStats.stunTimer);
        player.playerStats.isStunned = false;
    }

    private IEnumerator Knockback(GameObject attacker)
    {
        Vector3 direction = transform.position - attacker.transform.position;
        direction = direction.normalized * player.playerStats.knockbackStrength;

        float dur = Time.time + player.playerStats.knockbackDuration;

        while (Time.time < dur)
        {
            agent.Move(direction * Time.deltaTime);
            yield return null;
        }
        yield return null;
    }

    private void LoadAnimations()
    {
        player.playerStats.isTryWalk = isWalkHeld;
    }

    #endregion
}
