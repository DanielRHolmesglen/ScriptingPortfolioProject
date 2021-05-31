using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineBuilder
{
    private string _startState;

    private readonly Dictionary<string, Func<StateMachine, IEnumerator>> _states =
        new Dictionary<string, Func<StateMachine, IEnumerator>>();

    public StateMachineBuilder RegisterState(string stateName,
        Func<StateMachine, IEnumerator> stateFunc)
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

    public StateMachine Build()
    {
        if (_startState is null)
        {
            throw new InvalidOperationException("No states have been registered.");
        }
        
        return new StateMachine(_startState, _states);
    }
}

public class StateMachine
{
    public Func<StateMachine, IEnumerator> CurrentState { get; private set; }

    private readonly Dictionary<string, Func<StateMachine, IEnumerator>> _states;

    internal StateMachine(string startState,
        Dictionary<string, Func<StateMachine, IEnumerator>> states)
    {
        _states = states;
        CurrentState = states[startState];
    }

    public void Transition(string stateName)
    {
        if (_states.TryGetValue(stateName, out var func))
        {
            CurrentState = func;
        }
        else
        {
            throw new InvalidOperationException($"State \"{stateName}\" has not been registered.");
        }
    }

    protected internal IEnumerator Run(MonoBehaviour behaviour)
    {
        while (true)
        {
            yield return behaviour.StartCoroutine(CurrentState(this));
        }
    }
}

public static class StateMachineExtensions
{
    public static void RunStateMachine(this MonoBehaviour behaviour, StateMachine machine)
    {
        behaviour.StartCoroutine(machine.Run(behaviour));
    }
} 