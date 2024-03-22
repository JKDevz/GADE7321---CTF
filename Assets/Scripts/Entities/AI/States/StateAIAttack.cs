using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateAIAttack : AIState, IState
{
    public void HandleState(ref GameState gameState)
    {
        if (!controller.player.Inventory.HasItem())
        {
            controller.agent.SetDestination(PlayerManager.Instance.GetPlayer().transform.position);
            controller.player.Attacking.Attack();
        }
        else
        {
            controller.agent.SetDestination(PlayerManager.Instance.GetPlayer().transform.position);
            controller.player.Attacking.UseItem();
        }

        controller.ChangeState(aiState.Search);
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
