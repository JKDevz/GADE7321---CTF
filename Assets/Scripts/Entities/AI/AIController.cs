using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIState
{
    protected AIController controller;
}

public class AIController : MonoBehaviour
{
    #region VARIABLES

    [Header("--- AI Controller Settings")]
    public aiState state;
    [HideInInspector] public aiState lastState;

    [Header("--- Exposed for testing Purposes")]
    public Transform target;
    public NavMeshAgent agent;
    public Player player;

    public StateAISearch searchSettings;

    [SerializeField] private StateAIAttack stateAttack;
    [SerializeField] private StateAIPursue statePursue;
    [SerializeField] private StateAIRetrieve stateRetrieve;
    [SerializeField] private StateAISearch stateSearch;

    private GameState gameState;

    //Booleans for decision making
    public bool playerHasItem { get; protected set; }
    public bool meHasItem { get; protected set; }

    private IState currentState;

    #endregion

    #region ENABLE DELEGATES

    private void OnEnable()
    {
        GameManager.onGameStateChanged += UpdateGameState;
        player.onDamage += OnDamage;
        GameManager.onRoundSetup += OnRespawned;
    }

    private void OnDisable()
    {
        GameManager.onGameStateChanged -= UpdateGameState;
        player.onDamage -= OnDamage;
        GameManager.onRoundSetup -= OnRespawned;
    }

    #endregion

    #region UNITY METHODS

    private void Awake()
    {
        if (TryGetComponent<Player>(out Player player))
        {
            this.player = player;
        }

        stateAttack = new StateAIAttack(this);
        statePursue = new StateAIPursue(this);
        stateRetrieve = new StateAIRetrieve(this);
        stateSearch = new StateAISearch(this);

        stateSearch.baseDangerRadius = player.playerSettings.baseDangerRadius;
        stateSearch.itemLookCooldown = searchSettings.itemLookCooldown;
        stateSearch.itemLookCooldown_Noise = searchSettings.itemLookCooldown_Noise;
        stateSearch.itemLookRadius_Noise = searchSettings.itemLookRadius_Noise;
        stateSearch.itemSearchChance = searchSettings.itemSearchChance;
        stateSearch.SearchRadius = searchSettings.SearchRadius;

        currentState = stateSearch;
    }

    private void Update()
    {
        ModifyStats();
        LoadAnimations();
        
        if (GameManager.Instance.GetGameState() == GameState.RoundPlaying)
        {
            currentState.HandleState(ref gameState);
        }
        else
        {
            agent.SetDestination(transform.position);
        }
    }

    #endregion

    #region METHODS

    public void ChangeState(aiState newState)
    {
        if (newState != state)
        {
            lastState = state;
            state = newState;

            switch (state)
            {
                case aiState.Search:
                    currentState = stateSearch;
                    break;
                case aiState.Pursue:
                    currentState = statePursue;
                    break;
                case aiState.Retrieve:
                    currentState = stateRetrieve;
                    break;
                case aiState.Attack:
                    currentState = stateAttack;
                    break;
            }
        }
    }

    private void UpdateGameState()
    {
        gameState = GameManager.Instance.GetGameState();
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

    private void LoadAnimations()
    {
        if (agent.velocity.magnitude > 1)
        {
            player.playerStats.isTryWalk = true;
        }
        else
        {
            player.playerStats.isTryWalk = true;
        }
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

    public void ResetSprintCooldown()
    {
        player.playerStats.sprintTimer = Time.time + player.playerStats.sprintCooldown;
    }

    private void ModifyStats()
    {
        float moveSpeed = player.playerStats.speed + player.playerStats.speedModifier;

        if (player.Inventory.HasFlag())
        {
            moveSpeed -= (moveSpeed * player.playerStats.flagCarryModifier);
        }

        if (player.playerStats.isStunned)
        {
            moveSpeed = moveSpeed / 2;
        }

        agent.speed = moveSpeed;
    }

    #endregion
}

public enum aiState
{
    Search,
    Pursue,
    Retrieve,
    Attack
}
