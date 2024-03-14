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

    protected bool isSprintHeld;
    protected bool isWalkHeld;
    protected bool isCarryingFlag;

    #endregion

    #region EVENTS

    public delegate void OnTrigger();
    public static event OnTrigger onTrigger;

    public delegate void OnMoving();
    public static event OnMoving onMoving;

    public delegate void OnSprinting();
    public static event OnSprinting onSprinting;

    #endregion

    #region DELEGATES

    private void OnEnable()
    {
        player.onDamage += OnDamage;
    }

    private void OnDisable()
    {
        player.onDamage -= OnDamage;
    }

    #endregion

    #region MAIN LOOP

    private void Awake()
    {
        if (TryGetComponent<Player>(out Player player))
        {
            this.player = player;
        }

        LoadValues();
    }

    private void Update()
    {
        TryMove();
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

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            isSprintHeld = true;
        }
        else if (ctx.canceled)
        {
            isSprintHeld = false;
        }
    }

    #endregion

    #region METHODS

    public void TryMove()
    {
        if (moveInput.magnitude != 0)//IF the player is moving
        {
            Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);
            float moveSpeed = player.playerStats.speed;

            if (isSprintHeld && player.playerStats.stamina > 0)
            {
                moveSpeed += (moveSpeed * player.playerStats.sprintModifier);
                player.playerStats.DrainStamina();
            }

            if (isCarryingFlag)
            {
                moveSpeed -= (moveSpeed * player.playerStats.flagCarryModifier);
            }

            if (player.playerStats.isStunned)
            {
                moveSpeed = moveSpeed / 2;
            }

            agent.velocity = move * moveSpeed;
        }
        
        //IF the player is not sprinting WHILE moving OR standing still
        if (!isSprintHeld || isSprintHeld && moveInput.magnitude == 0)
        {
            player.playerStats.RegenStamina();//Regenerate stamina
        }
    }

    private void OnDamage()
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
    }

    private IEnumerator Stunned()
    {
        player.playerStats.stunTimer = Time.time + player.playerSettings.stunDuration;
        player.playerStats.isStunned = true;
        yield return new WaitUntil(() => Time.time > player.playerStats.stunTimer);
        player.playerStats.isStunned = false;
    }

    private void LoadAnimations()
    {
        player.playerStats.isTryWalk = isWalkHeld;
        player.playerStats.isSprinting = isSprintHeld;
    }

    public void LoadValues()
    {
        player.playerStats = new PlayerStats();

        player.playerStats.stamina = player.playerSettings.stamina;
        player.playerStats.speed = player.playerSettings.speed;
        player.playerStats.sprintModifier = player.playerSettings.sprintModifier;
        player.playerStats.flagCarryModifier = player.playerSettings.flagCarryModifier;

        player.playerStats.stamina = player.playerSettings.stamina;
        player.playerStats.staminaTickDelay = player.playerSettings.staminaTickDelay;
        player.playerStats.staminaTickRegen = player.playerSettings.staminaTickRegen;

        agent.angularSpeed = player.playerSettings.angularSpeed;
        agent.autoBraking = player.playerSettings.autoBraking;
        agent.acceleration = player.playerSettings.acceleration;

        agent.radius = player.playerSettings.radius;
        agent.height = player.playerSettings.height;
    }

    #endregion
}
