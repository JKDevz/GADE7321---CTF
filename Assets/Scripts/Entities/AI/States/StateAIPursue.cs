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
        controller.TrySprint(25, 10, 60);
        controller.CheckSpeed();

        controller.agent.SetDestination(PlayerManager.Instance.GetPlayer().transform.position);//Pathfind my score zone

        if (controller.player.Inventory.HasFlag())//If I have my flag and I am pursuing the player
        {
            controller.player.Inventory.DropFlag();//Drop my flag
        }

        if (!FlagManager.Instance.playerHasFlag)//IF the player no longer has their flag, go back to what I was doing
        {
            controller.ChangeState(aiState.Search);
        }
        else if (Vector3.Distance(PlayerManager.Instance.GetPlayer().transform.position, controller.transform.position)
            <= controller.player.playerSettings.attackRange)//IF player is close, attack them
        {
            controller.ChangeState(aiState.Attack);
        }
    }

    public void EnterState()
    {

    }

    public void ExitState()
    {

    }
}
