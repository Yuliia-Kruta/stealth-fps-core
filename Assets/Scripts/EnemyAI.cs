using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // The amount of time this enemy is stunned for
    private float stunTimer = 0.0f;
    // The position the NavMeshAgent should be pathing towards
    private Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // onCollisionEnter()
        // if collision is with patrol node:
            // Set the destination to the position of the next node in the current route
            // pathfinding.updateDestination(followPatrolRoute.findNextNode().transform.position)
}
