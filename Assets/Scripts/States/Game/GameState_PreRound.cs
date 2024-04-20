using System.Collections;
using UnityEngine;

public class GameState_PreRound : State
{
    public GameState_PreRound(StateMachine stateMachine) : base(stateMachine) { }

    public override string GetName()
    {
        return "GameState_PreRound";
    }

    public override void TryStateTransition(IState newState)
    {
        ExecuteStateTransition(newState);
    }
}
