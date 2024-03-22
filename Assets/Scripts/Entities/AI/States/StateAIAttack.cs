using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateAIAttack : AIState, IState
{
    private float itemUseWait;

    public void HandleState(ref GameState gameState)
    {
        if (!controller.player.Inventory.HasItem())
        {
            controller.player.Attacking.Attack();
        }
        else if (controller.player.Inventory.HasItem())
        {
            controller.ResetItemUseWait();
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
