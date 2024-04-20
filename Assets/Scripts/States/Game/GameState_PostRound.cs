using System.Collections;
using UnityEngine;

public class GameState_PostRound : State
{
    public GameState_PostRound(StateMachine stateMachine) : base(stateMachine) { }

    public override string GetName()
    {
        return "GameState_PostRound";
    }

    public override void TryStateTransition(IState newState)
    {
        ExecuteStateTransition(newState);
    }
}
