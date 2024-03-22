using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRoundSetup : IState
{
    public UIManager uiManager;

    public void HandleState(ref GameState currentState)
    {
        //Spawn Flags
        //Spawn Players

        //3 second countdown
        //Drop the base walls

        GameManager.Instance.ChangeState(GameState.RoundPlaying);
    }

    public void EnterState()
    {
        GameManager.onRoundSetup?.Invoke();
    }

    public void ExitState()
    {

    }
}
