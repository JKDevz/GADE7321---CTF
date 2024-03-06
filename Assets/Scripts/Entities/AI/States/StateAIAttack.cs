using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateAIAttack : AIState, IState
{
    public void HandleState(ref GameState currentState)
    {
        throw new System.NotImplementedException();
    }

    public StateAIAttack(AIController controller)
    {
        this.controller = controller;
    }

    public void EnterState()
    {

    }

    public void ExitState()
    {

    }
}
