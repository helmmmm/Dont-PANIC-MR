using System.Collections;
using UnityEngine;

public class GameState_Pause : State
{
    public GameState_Pause(StateMachine stateMachine) : base(stateMachine) { }

    public override string GetName()
    {
        return "GameState_Pause";
    }

    public override void TryStateTransition(IState newState)
    {
        ExecuteStateTransition(newState);
    }
}
