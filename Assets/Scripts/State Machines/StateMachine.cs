using System.Runtime.CompilerServices;
using UnityEngine;

public class StateMachine
{
    private IState _currentState;

    protected void SetInitialState(IState state)
    {
        _currentState = state;
        _currentState.TryStateTransition(state);
    }

    public IState GetCurrentState()
    {
        return _currentState;
    }

    public void TryChangeState(State newState)
    {
        Debug.Log("Changing state to: " + newState.GetName() + "\n");
        _currentState.TryStateTransition(newState);
    }

    public void ExecuteStateTransition(IState newState)
    {
        _currentState = newState;
        newState.Enter();
    }
}