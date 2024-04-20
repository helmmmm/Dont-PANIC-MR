using System.Collections;
using UnityEngine;

public class GameState_GameOver : State
{
    public GameState_GameOver(StateMachine stateMachine) : base(stateMachine) { }

    public override string GetName()
    {
        return "GameState_GameOver";
    }

    public override void TryStateTransition(IState newState)
    {
        ExecuteStateTransition(newState);
    }
}
