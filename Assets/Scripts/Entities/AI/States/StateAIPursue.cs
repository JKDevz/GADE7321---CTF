using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateAIPursue : AIState, IState
{
    #region CONSTRUCTOR

    public StateAIPursue(AIController controller)
    {
        this.controller = controller;
    }

    #endregion

    public void HandleState(ref GameState gameState)
    {
        
    }

    public void EnterState()
    {

    }

    public void ExitState()
    {

    }
}
