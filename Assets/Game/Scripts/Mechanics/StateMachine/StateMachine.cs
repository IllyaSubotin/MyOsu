
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StateMachine
{
    private State _currentState;
    private State _previousState;
    private DiContainer _container;

    [Inject]
    public void Construct(DiContainer container)
    {
        _container = container;
    }

    public void ChangeState<T>() where T : State
    {
        var newState = _container.Resolve<T>();
        ChangeState(newState);
    }

    public void ChangeState(State newState)
    {
        _currentState?.Exit();

        _previousState = _currentState;
        _currentState = newState;

        _currentState?.Enter();
    }
}
