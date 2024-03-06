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

    [Header("--- Exposed for testing Purposes")]
    public Transform target;
    public NavMeshAgent agent;
    public PlayerInventory myInv;

    private StateAIAttack stateAttack;
    private StateAIPursue statePursue;
    private StateAIRetrieve stateRetrieve;
    private StateAISearch stateSearch;

    private GameState gameState;

    //Booleans for decision making
    public bool playerHasFlag { get; protected set; }
    public bool meHasFlag { get; protected set; }
    public bool playerHasItem { get; protected set; }
    public bool meHasItem { get; protected set; }

    private IState currentState;

    #endregion

    #region ENABLE DELEGATES

    private void OnEnable()
    {
        GameManager.onGameStateChanged += UpdateGameState;
        Flag.onFlagPickup += OnFlagPickup;
        Flag.onFlagDrop += OnFlagDrop;
    }

    private void OnDisable()
    {
        GameManager.onGameStateChanged -= UpdateGameState;
        Flag.onFlagPickup -= OnFlagPickup;
        Flag.onFlagDrop -= OnFlagDrop;
    }

    #endregion

    #region UNITY METHODS

    private void Awake()
    {
        stateAttack = new StateAIAttack(this);
        statePursue = new StateAIPursue(this);
        stateRetrieve = new StateAIRetrieve(this);
        stateSearch = new StateAISearch(this);

        currentState = stateSearch;
    }

    private void Update()
    {
        //switch (state)
        //{
        //    case aiState.Search:
        //        stateSearch.HandleState(ref gameState);
        //        break;
        //    case aiState.Pursue:
        //        break;
        //    case aiState.Retrieve:
        //        break;
        //    case aiState.Attack:
        //        break;
        //}

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
            state = newState;
        }
    }

    private void UpdateGameState()
    {
        gameState = GameManager.Instance.GetGameState();
    }

    private void OnFlagPickup(FlagType type)
    {
        if (type == FlagType.Blue)
        {
            playerHasFlag = true;
        }
    }

    private void OnFlagDrop(FlagType type)
    {
        if (type == FlagType.Blue)
        {
            playerHasFlag = false;
        }
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
