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
        controller.TrySprint(25, 10, 8);
        //controller.CheckSpeed();

        controller.agent.SetDestination(ScoreZoneManager.Instance.redScoreZone.transform.position);//Pathfind to my score zone

        if (FlagManager.Instance.aiHasFlag && !FlagManager.Instance.playerHasFlag)//If I have my flag AND the player doesn't have theirs -> Enter Retrieve State
        {
            controller.ChangeState(aiState.Retrieve);
        }
        else if (FlagManager.Instance.aiHasFlag && FlagManager.Instance.playerHasFlag)//If both I and the player have our flags
        {
            //If I am closer to my base than the player
            //THEN try and score -> Enter Retrieve State
            if (Vector3.Distance(controller.transform.position, ScoreZoneManager.Instance.redScoreZone.transform.position)
                > Vector3.Distance(PlayerManager.Instance.GetPlayer().transform.position, ScoreZoneManager.Instance.blueScoreZone.transform.position))
            {
                return;
            }
            else //ELSE if the player is closer to their base -> Enter Pursue State
            {
                controller.ChangeState(aiState.Pursue);
            }
        }
        else if (Vector3.Distance(PlayerManager.Instance.GetPlayer().transform.position, controller.transform.position) <= controller.player.playerSettings.attackRange)
        {
            controller.ChangeState(aiState.Attack);
        }
        else //If the player has their flag AND I don't have mine -> Enter Pursue State
        {
            controller.ChangeState(aiState.Pursue);
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
