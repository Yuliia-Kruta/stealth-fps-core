using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAITest : MonoBehaviour
{
    // The amount of time this enemy is stunned for
    private float stunTimer = 0.0f;

    // Components
    private Pathfinding pathfinding;
    private FollowPatrolRoute followPatrolRoute;
    private NavMeshAgent navMeshAgent;
    
    private enum EnemyState { Patrolling, Investigating, Stunned }
    private EnemyState currentState = EnemyState.Patrolling;
    
    private Vector3 noisePosition;
    // Distance to consider "arrived"
    private float arrivedThreshold = 1.5f; 

    // Start is called before the first frame update
    void Start()
    {
        // Components
        pathfinding = GetComponent<Pathfinding>();
        followPatrolRoute = GetComponent<FollowPatrolRoute>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Set the destination to the patrol route
        pathfinding.updateDestination(followPatrolRoute.destinationNode.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (stunTimer > 0f)
        {
            
            stunTimer -= Time.deltaTime;
            if (!navMeshAgent.isStopped)
                navMeshAgent.isStopped = true;
        }
        else
        {
            if (navMeshAgent.isStopped)
                navMeshAgent.isStopped = false;
            
            // Check if we are investigating and arrived at noise location
            if (currentState == EnemyState.Investigating)
            {
                float distanceToNoise = Vector3.Distance(transform.position, noisePosition);
                if (distanceToNoise <= arrivedThreshold)
                {
                    Debug.Log("Enemy investigated noise and found nothing. Returning to patrol.");
                    currentState = EnemyState.Patrolling;
                    pathfinding.updateDestination(followPatrolRoute.destinationNode.transform.position);
                }
            }
        }
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

        // We check if the collision object has a PatrolNode before comparing it 
        PatrolNode node = collidedObject.GetComponent<PatrolNode>();
        // If we collided with our destination patrol node
        if (node != null && node == followPatrolRoute.destinationNode)
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
    
    // Method to stun the enemy
    public void Stun(float duration)
    {
        // Apply stun if not already stunned
        if (stunTimer <= 0f) 
        {
            stunTimer = duration;
            Debug.Log($"Enemy stunned for {duration} seconds.");
        }
    }

    // Behaviour that runs when noise is received
    /*public void OnNoiseReceived(Vector3 noisePosition)
    {
        Debug.Log("I hear noise!!!");
        Debug.Log(noisePosition);
    }*/
    
    /*public void OnNoiseReceived(Vector3 noisePosition)
    {
        if (stunTimer > 0f) return;

        Debug.Log($"Enemy hears noise at {noisePosition}");
        pathfinding.updateDestination(noisePosition);
    }*/
    
    public void OnNoiseReceived(Vector3 noisePos)
    {
        if (stunTimer > 0f) return;

        Debug.Log($"Enemy hears noise at {noisePos}");
        currentState = EnemyState.Investigating;
        noisePosition = noisePos;
        pathfinding.updateDestination(noisePosition);
    }
}
