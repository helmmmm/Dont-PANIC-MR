using System.Collections;
using UnityEngine;

public class GameState_MidRound : State
{
    public GameState_MidRound(StateMachine stateMachine) : base(stateMachine) { }

    public override string GetName()
    {
        return "GameState_MidRound";
    }

    public override void TryStateTransition(IState newState)
    {
        ExecuteStateTransition(newState);
    }
}
