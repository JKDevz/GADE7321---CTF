using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGameStart : IState
{
    public void HandleState(ref GameState currentState)
    {
        //Play intro, showcase the map, etc.
        GameManager.Instance.ChangeState(GameState.RoundSetup);
    }

    public void EnterState()
    {

    }

    public void ExitState()
    {

    }
}
