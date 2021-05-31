using System;
using System.Collections;
using System.Collections.Generic;

public class StateMachineBuilder
{
    private string _startState;

    private readonly Dictionary<string, Func<StateMachine, UnityEngine.YieldInstruction>> _states =
        new Dictionary<string, Func<StateMachine, UnityEngine.YieldInstruction>>();

    public StateMachineBuilder RegisterState(string stateName,
        Func<StateMachine, UnityEngine.YieldInstruction> stateFunc)
    {
        if (_states.ContainsKey(stateName))
        {
            throw new InvalidOperationException($"State \"{stateName}\" has already been registered.");
        }

        _states[stateName] = stateFunc;

        if (_startState is null)
        {
            _startState = stateName;
        }

        return this;
    }

    public StateMachineBuilder SetStartState(string stateName)
    {
        if (_states.ContainsKey(stateName))
        {
            _startState = stateName;
        }
        else
        {
            throw new InvalidOperationException($"State \"{stateName}\" has not been registered.");
        }

        return this;
    }

    public IEnumerator Build()
    {
        if (_startState is null)
        {
            throw new InvalidOperationException("No states have been registered.");
        }

        var machine = new StateMachine(_startState, _states);

        return machine.Run();
    }
}

public class StateMachine
{
    private Func<StateMachine, UnityEngine.YieldInstruction> _currentState;
    private readonly Dictionary<string, Func<StateMachine, UnityEngine.YieldInstruction>> _states;

    internal StateMachine(string startState,
        Dictionary<string, Func<StateMachine, UnityEngine.YieldInstruction>> states)
    {
        _states = states;
        _currentState = states[startState];
    }

    public void Transition(string stateName)
    {
        if (_states.TryGetValue(stateName, out var func))
        {
            _currentState = func;
        }
        else
        {
            throw new InvalidOperationException($"State \"{stateName}\" has not been registered.");
        }
    }

    internal IEnumerator Run()
    {
        while (true)
        {
            yield return _currentState(this);
        }
    }
}