using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class StateAISearch : AIState, IState
{
    #region VARIABLES

    private Transform target;

    private float itemLookRadius;
    private float itemLookWait;
    private ItemSpawner targetSpawner;

    private bool isFindingItem;

    #endregion

    #region CONSTRUCTOR

    public StateAISearch(AIController controller)
    {
        this.controller = controller;
    }

    #endregion

    public void HandleState(ref GameState gameState)
    {

        FindFlag();
        controller.agent.SetDestination(target.position);//Pathfind to the nearest flag

        //IF I can find an item, override my destination to the nearest item
        if (!isFindingItem && !controller.player.Inventory.HasItem())
        {
            //controller.ResetItemUseWait();
            FindItem();
        }
        else if (isFindingItem && targetSpawner.HasItem() && !controller.player.Inventory.HasItem())
        {
            //controller.ResetItemUseWait();
            controller.agent.SetDestination(targetSpawner.item.transform.position);//Pathfind to the nearest flag
        }
        else if (!targetSpawner.HasItem() || controller.player.Inventory.HasItem())
        {
            isFindingItem = false;
        }

        if (FlagManager.Instance.aiHasFlag && !FlagManager.Instance.playerHasFlag)//If I have my flag AND the player doesn't have theirs -> Enter Retrieve State
        {
            isFindingItem = false;
            controller.ChangeState(aiState.Retrieve);
        }
        else if (FlagManager.Instance.aiHasFlag && FlagManager.Instance.playerHasFlag)//If both I and the player have our flags
        {
            //If I am closer to my base than the player
            //THEN try and score -> Enter Retrieve State
            if (Vector3.Distance(controller.transform.position, ScoreZoneManager.Instance.redScoreZone.transform.position) 
                > Vector3.Distance(PlayerManager.Instance.GetPlayer().transform.position, ScoreZoneManager.Instance.blueScoreZone.transform.position))
            {
                isFindingItem = false;
                controller.ChangeState(aiState.Retrieve);
            }
            else //ELSE if the player is closer to their base -> Enter Pursue State
            {
                isFindingItem = false;
                controller.ChangeState(aiState.Pursue);
            }
        }
        else if (controller.player.Inventory.HasItem())
        {
            isFindingItem = false;
        }
        else if (!FlagManager.Instance.aiHasFlag && FlagManager.Instance.playerHasFlag)//If the player has their flag AND I don't have mine -> Enter Pursue State
        {
            isFindingItem = false;
            controller.ChangeState(aiState.Pursue);
        }
        else if (Vector3.Distance(PlayerManager.Instance.GetPlayer().transform.position, controller.transform.position) <= controller.player.playerStats.meleeRange)
        {
            controller.ChangeState(aiState.Attack);
        }

        if (controller.player.Inventory.HasItem() && Time.time > controller.itemUseWait)
        {
            controller.ChangeState(aiState.Attack);
        }
    }

    public void EnterState()
    {
        FindFlag();
    }

    private void FindFlag()
    {
        //If the Players' flag is not at the Red Base
        if (FlagManager.Instance.blueFlag.flagState == FlagState.Exposed)
        {
            //If the Players' flag is closer, go for that
            //Else, go after my own flag
            if (Vector3.Distance(controller.agent.transform.position, FlagManager.Instance.blueFlag.transform.position) 
                < Vector3.Distance(controller.agent.transform.position, FlagManager.Instance.redFlag.transform.position))
            {
                target = FlagManager.Instance.blueFlag.transform;
            }
        }
        else
        {
            target = FlagManager.Instance.redFlag.transform;
        }
    }

    private void FindItem()
    {
        //If I can look for a power-up AND I do not have a power-up
        if (!controller.player.Inventory.HasItem() && Time.time > itemLookWait)
        {
            ResetItemLookTimer();

            //IS the player close to my base?
            if (Vector3.Distance(PlayerManager.Instance.GetPlayer().transform.position, ScoreZoneManager.Instance.redScoreZone.transform.position) < controller.player.playerSettings.baseDangerRadius)
            {
                //Don't look for a power-up
                isFindingItem = false;
                return;
            }

            SetSearchRadius();

            foreach (ItemSpawner spawner in ItemManager.Instance.itemSpawners)
            {
                //IS there a power-up nearby?
                if (spawner.HasItem() && Vector3.Distance(controller.agent.transform.position, spawner.transform.position) <= itemLookRadius)
                {
                    //THEN do a random roll to see if I want to go for it
                    if (Random.Range(0, controller.itemSearchChance.y) <= controller.itemSearchChance.x)
                    {
                        target = spawner.item.transform;
                        targetSpawner = spawner;
                        isFindingItem = true;//Yes I AM looking for a power-up thank you :D
                        return;
                    }
                }
            }
        }
    }

    private void ResetItemLookTimer()
    {
        itemLookWait = Time.time + controller.itemLookCooldown + Random.Range(controller.itemLookCooldown_Noise.x, controller.itemLookCooldown_Noise.y);
    }

    private void SetSearchRadius()
    {
        itemLookRadius = controller.SearchRadius + Random.Range(controller.itemLookRadius_Noise.x, controller.itemLookRadius_Noise.y);
    }

    public void ExitState()
    {

    }
}
