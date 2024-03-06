using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{

    public abstract void HandleState(ref GameState currentState);

    public abstract void EnterState();

    public abstract void ExitState();
}
