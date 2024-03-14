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
            lastState = state;
            state = newState;

            Debug.Log("Changed to " + newState.ToString() + " State!");
        }
    }

    private void UpdateGameState()
    {
        gameState = GameManager.Instance.GetGameState();
    }

    public void CheckSpeed()
    {
        agent.speed = player.playerSettings.speed;

        if (player.playerStats.isSprinting && player.playerStats.stamina > 0)
        {
            agent.speed += (agent.speed * player.playerStats.sprintModifier);
        }

        if (player.Inventory.HasFlag())
        {
            agent.speed -= (agent.speed * player.playerStats.flagCarryModifier);
        }
    }

    public void TrySprint(int minStamina, int randMax, int rollWin)
    {
        if (player.playerStats.stamina > 0 && Time.time > player.playerStats.sprintTimer)//IF I can try and sprint, do so AND I have enough stamina...lol
        {
            if (player.playerStats.stamina <= minStamina)//Do I have enough DESIRED stamina?
            {
                player.playerStats.isSprinting = false;//No? Stop Sprinting
            }
            else if (Random.Range(0, randMax) >= rollWin)//I have enough stamina, now lets give a random try to sprint
            {
                player.playerStats.isSprinting = true;//Start sprinting
                ResetSprintCooldown();//Reset the wait before I can try again
            }
            else
            {
                player.playerStats.isSprinting = false;//Stop sprinting
            }
        }
        else
        {
            player.playerStats.isSprinting = false;//Stop sprinting
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

        if (agent.velocity.magnitude > 0 && player.playerStats.isSprinting)
        {
            moveSpeed += (moveSpeed * player.playerStats.sprintModifier);
            player.playerStats.DrainStamina();
        }
        else if (!player.playerStats.isSprinting || player.playerStats.isSprinting && agent.velocity.magnitude == 0)//IF the player is not sprinting WHILE moving OR standing still
        {
            player.playerStats.RegenStamina();//Regenerate stamina
        }

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
