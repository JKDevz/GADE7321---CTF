using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateRoundFinished : IState
{
    public void HandleState(ref GameState currentState)
    {

    }

    public void EnterState()
    {
        if (GameManager.Instance.playerScore == 5 || GameManager.Instance.aiScore == 5)
        {
            GameManager.Instance.ChangeState(GameState.GameFinished);
            return;
        }

        GameManager.Instance.ChangeState(GameState.RoundSetup);
    }

    public void ExitState()
    {

    }
}
