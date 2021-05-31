using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public static class RepeatForeverExtension
{
    // I have to implement this myself because C# is bad.
    public static IEnumerable<T> RepeatForever<T>(this IEnumerable<T> source)
    {
        var enumerable = source.ToList();
        while (true)
        {
            foreach (var item in enumerable)
            {
                yield return item;
            }
        }
    }
}

public class BetterAIPatrol : MonoBehaviour
{
    private IEnumerator<Vector3> _waypoints;
    private NavMeshAgent _agent;
    
    private IEnumerator Idle(StateMachine machine)
    {
        _agent.isStopped = true;
        yield return new WaitForSeconds(5.0f);
        machine.Transition("patrol");
    }
    
    private IEnumerator Patrol(StateMachine machine)
    {
        while (true)
        {
            _agent.isStopped = false;
            if (!_agent.pathPending || _agent.remainingDistance < 0.5f)
            {
                yield return new WaitForSeconds(Random.Range(0.1f, 1.5f));
                _waypoints.MoveNext();
                _agent.destination = _waypoints.Current;
            }
            
            // We're bored now, so be idle for a bit
            if (!(Random.Range(0f, 1f) > 0.8f)) continue;
            machine.Transition("idle");
            yield break;
        }
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _waypoints = GetComponentsInChildren<Transform>()
            .Where(waypoint => waypoint.CompareTag("Waypoint"))
            .Select(node => node.position)
            .RepeatForever()
            .GetEnumerator();
        
        var stateMachine = new StateMachineBuilder()
            .RegisterState("idle", Idle)
            .RegisterState("patrol", Patrol)
            .Build();

        this.RunStateMachine(stateMachine);
    }
}
