using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIPatrol : MonoBehaviour
{
    enum States {IDLE,PATROL,CHASE }
    States currentState;


    NavMeshAgent agent;
    GameObject target;

    [SerializeField] float sightRange, attackRange;
    List<Vector3> Nodes = new List<Vector3>();
    int destinationNode = 0;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");

        currentState = States.IDLE;
        StartCoroutine(SM());
        CollectNodes();
        
    }

    IEnumerator SM()
    {
        while (true)
        { 
            yield return StartCoroutine(currentState.ToString());
        }
    }

    IEnumerator IDLE()
    {   //Enter state,run first logic
        Debug.Log("just setting here...");
        //execute state behaviour
        while (currentState == States.IDLE)
        {
            yield return new WaitForSeconds(1.0f);

            Debug.Log("waiting...");

            yield return new WaitForSeconds(2.0f);

            Debug.Log("...booooooed");

            currentState = States.PATROL;
            yield return null;
        }
        //exit the state
    }

    IEnumerator PATROL()
    {   //Enter state,run first logic
        Debug.Log("guess I will go to work then...");
        //execute state behaviour
        while (currentState == States.PATROL)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                yield return new WaitForSeconds(2.0f);
                GoToNextNode();

            }
            yield return null;
        }
        //exit the state
    }

    IEnumerator CHASE()
    {
        Debug.Log("STOP YOU HAVE ");
        //ENTER STATE
        agent.SetDestination(transform.position);
        transform.LookAt(target.transform.position);

        yield return new WaitForSeconds(1f);
        while (currentState == States.PATROL)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                agent.SetDestination(target.transform.position);
            }
            yield return null;
        }

    }

    void CheckForPlayer()
    {
        float gap = Vector3.Distance(transform.position, target.transform.position);

        if (gap < sightRange)
        {
            currentState = States.CHASE;
        }
        else
        {
            currentState = States.PATROL;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextNode();
        }
        
    }

    void GoToNextNode()
    { //if there is no node
        if (Nodes.Count == 0)
        {
            return; 
        }
        //set destination to current node
        agent.destination = Nodes[destinationNode];
        //choose the next node in the list and loop back to zero if max is reached.
        destinationNode = (destinationNode + 1) % Nodes.Count;
    }
    void CollectNodes()
    {
        foreach (Transform waypoint in GetComponentsInChildren<Transform>())
        {
            if (waypoint.CompareTag("Waypoint"))
            {
                Nodes.Add(waypoint.position);
                Destroy(waypoint.gameObject);
            }
        }
    }

     void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        foreach (Transform waypoint in GetComponentsInChildren<Transform>())
        {
            if (waypoint.CompareTag("Waypoint"))
            {
                Gizmos.DrawSphere(waypoint.position, 0.3f);
            }
        }
    }
}
