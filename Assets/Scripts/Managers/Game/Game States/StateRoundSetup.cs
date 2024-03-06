using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRoundSetup : IState
{
    public UIManager uiManager;

    public void HandleState(ref GameState currentState)
    {
        GameManager.onRoundSetup?.Invoke();
        //Spawn Flags
        //Spawn Players

        //3 second countdown
        //Drop the base walls

        currentState = GameState.RoundPlaying;
    }

    public void EnterState()
    {

    }

    public void ExitState()
    {

    }
}
