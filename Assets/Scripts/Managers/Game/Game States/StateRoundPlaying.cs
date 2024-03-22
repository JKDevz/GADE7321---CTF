using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRoundPlaying : IState
{
    public void HandleState(ref GameState currentState)
    {
        
    }

    public void EnterState()
    {
        GameManager.onRoundPlaying?.Invoke();//Turn off barriers, enable power-up spawning, etc
        GameManager.Instance.currentRound++;
    }

    public void ExitState()
    {

    }
}
