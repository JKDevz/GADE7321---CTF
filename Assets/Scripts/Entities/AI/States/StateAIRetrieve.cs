using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateAIRetrieve : AIState, IState
{
    #region CONSTRUCTOR

    public StateAIRetrieve(AIController controller)
    {
        this.controller = controller;
    }

    #endregion

    public void HandleState(ref GameState currentState)
    {
        
    }

    public void EnterState()
    {

    }

    public void ExitState()
    {

    }
}
