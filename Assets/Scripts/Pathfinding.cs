using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    // The position the NavMeshAgent should be pathing towards
    private Vector3 destination;

    // Components
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    
    void Start()
    {
        // Components
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Updates the NavMeshAgent's destination
    public void updateDestination(Vector3 newDestination)
    {
        destination = newDestination;
        navMeshAgent.destination = destination;
    }
}
