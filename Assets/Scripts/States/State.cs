using System;
using Unity.VisualScripting;
using UnityEngine;

public interface IState
{
    void Enter();
    void Exit();

    void TryStateTransition(IState newState);
    string GetName();
}

public class State : IState
{
    private StateMachine MyStateMachine { get; set; }

    public State(StateMachine stateMachine)
    {
        MyStateMachine = stateMachine;
    }

    public void Enter()
    {
        OnEnter?.Invoke();
    }

    public void Exit()
    {
        OnExit?.Invoke();
    }

    public event Action OnEnter;
    public event Action OnExit;

    public virtual void TryStateTransition(IState newState)
    {
        MyStateMachine.ExecuteStateTransition(newState);
    }

    public virtual string GetName()
    {
        throw new NotImplementedException();
    }

    protected void ExecuteStateTransition(IState newState)
    {
        Exit();
        MyStateMachine.ExecuteStateTransition(newState);
    }
}