using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // The amount of time this enemy is stunned for
    private float stunTimer = 0.0f;

    // If the player is in the enemy's vision
    private bool canSeePlayer = false;

    // A reference to the player object, populated when this enemy first sees the player
    private GameObject playerObject;

    // Components
    private Pathfinding pathfinding;
    private FollowPatrolRoute followPatrolRoute;
    private NavMeshAgent navMeshAgent;

    private enum EnemyState
    {
        Patrolling,
        Investigating,
        Stunned
    }

    private EnemyState currentState = EnemyState.Patrolling;

    private Vector3 noisePosition;

    // Distance to consider "arrived"
    private float arrivedThreshold = 1.5f;
    
    // Range of "line-of-sight"
    private float visionRange = 15f;


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
        // Decreasing the stun timer
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
            
            // Check line of sight
            if (playerObject != null)
            {
                CheckLineOfSight();
            }
            
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

        // Path toward the player if they're in the enemy's vision
        if (canSeePlayer && playerObject != null)
        {
            pathfinding.updateDestination(playerObject.transform.position);
        }

        // If the NavMeshAgent isn't walking, continue patrolling
        /*else if (navMeshAgent.velocity == new Vector3(0, 0, 0) && navMeshAgent.isStopped == false)
        {
            pathfinding.updateDestination(followPatrolRoute.destinationNode.transform.position);
        }*/
        // Return to patrol if not doing something else
        else if (currentState != EnemyState.Investigating && currentState != EnemyState.Stunned)
        {
            currentState = EnemyState.Patrolling;
            pathfinding.updateDestination(followPatrolRoute.destinationNode.transform.position);
        }
    }


    void OnTriggerEnter(Collider collision)
    {
        // Don't react to any collisions when we're stunned
        if (stunTimer > 0f)
            return;

        // Get the object that we collided with
        GameObject collidedObject = collision.gameObject;

        // Try to get components from the collided object to figure out what that object is
        PatrolNode node = collidedObject.GetComponent<PatrolNode>();
        PlayerController player = collidedObject.GetComponent<PlayerController>();

        // If we collided with our destination patrol node
        if (node != null && node == followPatrolRoute.destinationNode)
        {
            // Find the next patrol node
            PatrolNode newDestinationNode =
                followPatrolRoute.findNodeByID(followPatrolRoute.destinationNode.nodeID + 1);

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

            return;
        }

        // If our vision overlapped with a player
        if (player != null)
        {
            // Save a reference to the player for later
            if (playerObject == null)
            {
                playerObject = collidedObject;
                Debug.Log("Got player object");
            }

            //canSeePlayer = true;
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

    void CheckLineOfSight()
    {
        if (playerObject == null)
            return;

        Vector3 directionToPlayer = (playerObject.transform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
        
        // If player is too far
        if (distanceToPlayer > visionRange)
        {
            Debug.Log("Player is not within line of sight range");
            canSeePlayer = false;
            // Forget the player
            playerObject = null; 
            return;
        }
        
        // Create a LayerMask to exclude the EnemyVision layer (VisionCone)
        int enemyVisionLayer = LayerMask.NameToLayer("EnemyVision");
        int layerMask = ~(1 << enemyVisionLayer);  

        // Cast a ray toward the player
        if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer, out RaycastHit hit, distanceToPlayer, layerMask))
        {
            // Check if the hit object is the player
            if (hit.collider.gameObject == playerObject)
            {
                canSeePlayer = true;
            }
            else
            {
                // If something blocks the view
                Debug.Log("No line of sight");
                canSeePlayer = false; 
                // Forget the player
                playerObject = null; 
            }
        }
    }


    public void OnNoiseReceived(Vector3 noisePos)
    {
        if (stunTimer > 0f) return;

        Debug.Log($"Enemy hears noise at {noisePos}");
        currentState = EnemyState.Investigating;
        noisePosition = noisePos;
        pathfinding.updateDestination(noisePosition);
    }
}