using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRoundPlaying : IState
{
    public void HandleState(ref GameState currentState)
    {
        GameManager.onRoundPlaying?.Invoke();//Turn off barriers, enable power-up spawning
    }

    public void EnterState()
    {

    }

    public void ExitState()
    {

    }
}
