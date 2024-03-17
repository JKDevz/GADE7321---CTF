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

    private StateAIAttack stateAttack;
    private StateAIPursue statePursue;
    private StateAIRetrieve stateRetrieve;
    private StateAISearch stateSearch;

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
    }

    private void OnDisable()
    {
        GameManager.onGameStateChanged -= UpdateGameState;
        player.onDamage -= OnDamage;
    }

    #endregion

    #region UNITY METHODS

    private void Awake()
    {
        if (TryGetComponent<Player>(out Player player))
        {
            this.player = player;
        }

        LoadValues();

        stateAttack = new StateAIAttack(this);
        statePursue = new StateAIPursue(this);
        stateRetrieve = new StateAIRetrieve(this);
        stateSearch = new StateAISearch(this);

        stateSearch.baseDangerRadius = player.playerSettings.baseDangerRadius;

        currentState = stateSearch;
    }

    private void Update()
    {
        ModifyStats();
        LoadAnimations();
        currentState.HandleState(ref gameState);
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

    public void ResetSprintCooldown()
    {
        player.playerStats.sprintTimer = Time.time + player.playerStats.sprintCooldown;
    }

    private void ModifyStats()
    {
        float moveSpeed = player.playerStats.speed;

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

    public void LoadValues()
    {
        player.playerStats = new PlayerStats();

        player.playerStats.speed = player.playerSettings.speed;
        player.playerStats.flagCarryModifier = player.playerSettings.flagCarryModifier;

        player.playerStats.stunDuration = player.playerSettings.stunDuration;
        player.playerStats.meleeRange = player.playerSettings.meleeRange;

        agent.angularSpeed = player.playerSettings.angularSpeed;
        agent.autoBraking = player.playerSettings.autoBraking;
        agent.acceleration = player.playerSettings.acceleration;

        agent.radius = player.playerSettings.radius;
        agent.height = player.playerSettings.height;
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
