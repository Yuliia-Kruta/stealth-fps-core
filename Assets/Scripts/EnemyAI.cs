using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // The amount of time this enemy is stunned for
    private float stunTimer = 0.0f;

    // Components
    private Pathfinding pathfinding;
    private FollowPatrolRoute followPatrolRoute;

    // Start is called before the first frame update
    void Start()
    {
        // Components
        pathfinding = GetComponent<Pathfinding>();
        followPatrolRoute = GetComponent<FollowPatrolRoute>();

        // Set the destination to the patrol route
        pathfinding.updateDestination(followPatrolRoute.destinationNode.transform.position);
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnCollisionEnter(Collision collision)
    {
        // Blank for now
    }

    void OnTriggerEnter (Collider collision) 
    {
        // Don't react to any collisions when we're stunned
        if (stunTimer > 0f)
            return;
        
        // Get the object that we collided with
        GameObject collidedObject = collision.gameObject;

        // If we collided with our destination patrol node
        if (collidedObject.GetComponent<PatrolNode>() == followPatrolRoute.destinationNode)
        {
            // Find the next patrol node
            PatrolNode newDestinationNode = followPatrolRoute.findNodeByID(followPatrolRoute.destinationNode.nodeID + 1);
            
            // If we did find a new patrol node
            if (newDestinationNode != null)
            {
                // Update the destination
                followPatrolRoute.destinationNode = newDestinationNode;
                pathfinding.updateDestination(newDestinationNode.transform.position);
            }
            else
            {
                Debug.LogError("Error! Enemy AI found no new destination node after colliding with a patrol node.");
            }
        }
        
    }
    // onCollisionEnter()
        // if collision is with patrol node:
            // if that patrol node is the same as followPatrolRoute.destinationNode
                // Set the destination to the position of the next node in the current route
                // pathfinding.updateDestination(followPatrolRoute.findNextNode().transform.position)
}
