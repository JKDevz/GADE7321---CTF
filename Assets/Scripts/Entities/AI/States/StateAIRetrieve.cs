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

    public void HandleState(ref GameState gameState)
    {
        Debug.Log(FlagManager.Instance.aiHasFlag);

        controller.agent.SetDestination(ScoreZoneManager.Instance.redScoreZone.transform.position);//Pathfind to my score zone

        if (FlagManager.Instance.aiHasFlag && FlagManager.Instance.playerHasFlag)//If both I and the player have our flags
        {
            Debug.Log("Both Players have their flag");
            //If the player is closer to their base than I am to mine -> Pursue the Player
            if (Vector3.Distance(controller.transform.position, ScoreZoneManager.Instance.redScoreZone.transform.position)
                < Vector3.Distance(PlayerManager.Instance.GetPlayer().transform.position, ScoreZoneManager.Instance.blueScoreZone.transform.position))
            {
                Debug.Log("Player is closer to their base!!!");
                controller.ChangeState(aiState.Pursue);
            }
            else
            {
                return;
            }
        }
        else if (!FlagManager.Instance.aiHasFlag && FlagManager.Instance.playerHasFlag)//If the player has their flag AND I don't have mine -> TRY Enter Pursue State
        {
            Debug.Log("Only Player has their flag");
            controller.ChangeState(aiState.Pursue);
        }
        else if (!FlagManager.Instance.aiHasFlag && !FlagManager.Instance.playerHasFlag)//If I don't have my flag AND neither does the player -> Enter Search State
        {
            Debug.Log("Neither Players have their flag");
            controller.ChangeState(aiState.Search);
        }

        //IF player is close, attack them
        if (Vector3.Distance(PlayerManager.Instance.GetPlayer().transform.position, controller.transform.position) <= controller.player.playerStats.meleeRange)
        {
            controller.ChangeState(aiState.Attack);
        }
    }

    public void FindScoreZone()
    {

    }

    public void EnterState()
    {

    }

    public void ExitState()
    {

    }
}
