using System.Collections;
using UnityEngine;

public class GameState_PreGame : State
{
    public GameState_PreGame(StateMachine stateMachine) : base(stateMachine) { }

    public override string GetName()
    {
        return "GameState_PreGame";
    }

    public override void TryStateTransition(IState newState)
    {
        ExecuteStateTransition(newState);
    }
}
