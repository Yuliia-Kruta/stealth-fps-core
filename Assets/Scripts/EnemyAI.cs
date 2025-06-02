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
    // If the enemy can have line of sight to the player
    private bool hasLineOfSight = false;

    // A reference to the player object, populated when this enemy first sees the player
    private GameObject playerObject;
    private PlayerController playerController;

    // Components
    private EnemyAI enemyAI;
    private Pathfinding pathfinding;
    private FollowPatrolRoute followPatrolRoute;
    private NavMeshAgent navMeshAgent;
    private MeleeScript meleeScript;

    private enum EnemyState
    {
        Patrolling,
        Investigating,
        Stunned,
        Chasing
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
        enemyAI = GetComponent<EnemyAI>();
        pathfinding = GetComponent<Pathfinding>();
        followPatrolRoute = GetComponent<FollowPatrolRoute>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        meleeScript = GetComponent<MeleeScript>();

        // Get a reference to the player
        playerObject = GameObject.FindObjectsOfType<PlayerController>()[0].gameObject;
        playerController = playerObject.GetComponent<PlayerController>();


        // Set the destination to the patrol route
        pathfinding.updateDestination(followPatrolRoute.destinationNode.transform.position);
    }


    // Update is called once per frame
    void Update()
    {
        // Check line of sight with the player
        string[] includedLayers = {"Obstacle"};
        hasLineOfSight = CheckLineOfSight2(transform.position + new Vector3(0f, 0.5f, 0f), playerObject.transform.position + new Vector3(0f, 0.5f, 0f), includedLayers, visionRange);


        // Tell the player if they have line of sight with this enemy
        if (hasLineOfSight)
        {
            if (playerController.enemiesWithLineOfSight.Contains(enemyAI) == false)
            {
                playerController.enemiesWithLineOfSight.Add(enemyAI);
            }
        }
        else 
        {
            if (playerController.enemiesWithLineOfSight.Contains(enemyAI) == true)
            {
                playerController.enemiesWithLineOfSight.Remove(enemyAI);
            }
        }


        // If this enemy is stunned
        if (stunTimer > 0f)
        {
            // Decrease the stun timer
            stunTimer -= Time.deltaTime;
            if (!navMeshAgent.isStopped)
                navMeshAgent.isStopped = true;
        }
        else
        {
            if (navMeshAgent.isStopped)
                navMeshAgent.isStopped = false;
            

            // If the enemy is chasing the player, allow them to keep track of the player with line of sight
            if (currentState == EnemyState.Chasing)
            {
                // Check line of sight between this enemy and the player
                if (hasLineOfSight)
                {
                    canSeePlayer = true;

                    // Tell the player we're chasing them with line of sight
                    if (playerController.enemiesChasingWithLineOfSight.Contains(enemyAI) == false)
                    {
                        playerController.enemiesChasingWithLineOfSight.Add(enemyAI);
                    }
                }
                else
                {
                    canSeePlayer = false;

                    // Tell the player we're no longer chasing them with line of sight
                    if (playerController.enemiesChasingWithLineOfSight.Contains(enemyAI) == true)
                    {
                        playerController.enemiesChasingWithLineOfSight.Remove(enemyAI);
                    }
                }
            }
            else
            {
                // Tell the player we're no longer chasing them with line of sight
                if (playerController.enemiesChasingWithLineOfSight.Contains(enemyAI) == true)
                {
                    playerController.enemiesChasingWithLineOfSight.Remove(enemyAI);
                }
            }
            
            
            // If the enemy is investigating and has arrived at its destination
            if (currentState == EnemyState.Investigating || currentState == EnemyState.Chasing)
            {
                float distanceToDestination = Vector3.Distance(transform.position, navMeshAgent.destination);
                if (distanceToDestination <= arrivedThreshold)
                {
                    Debug.Log("Enemy investigated noise and found nothing. Returning to patrol.");
                    currentState = EnemyState.Patrolling;
                    pathfinding.updateDestination(followPatrolRoute.destinationNode.transform.position);
                }
            }
            
        }

        // Path toward the player if they're in the enemy's vision
        if (canSeePlayer)
        {
            pathfinding.updateDestination(playerObject.transform.position);
        }

        // If the NavMeshAgent isn't walking, continue patrolling
        /*else if (navMeshAgent.velocity == new Vector3(0, 0, 0) && navMeshAgent.isStopped == false)
        {
            pathfinding.updateDestination(followPatrolRoute.destinationNode.transform.position);
        }*/
        
        // Return to patrol if not doing something else
        else if (currentState != EnemyState.Investigating && currentState != EnemyState.Stunned && currentState != EnemyState.Chasing)
        {
            currentState = EnemyState.Patrolling;
            pathfinding.updateDestination(followPatrolRoute.destinationNode.transform.position);
        }

        Debug.DrawRay(navMeshAgent.destination + new Vector3(-1f, 0, -1f), new Vector3(2f, 0, 2f), Color.blue, 0.025f);
        Debug.DrawRay(navMeshAgent.destination + new Vector3(-1f, 0, 1f), new Vector3(2f, 0, -2f), Color.blue, 0.025f);
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
                Debug.LogError("<color='red'>Error!</color> Enemy AI found no new destination node after colliding with a patrol node.");
            }

            return;
        }

        // If our vision overlapped with a player
        if (player != null)
        {
            currentState = EnemyState.Chasing;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Don't react to any collisions when we're stunned
        if (stunTimer > 0f)
            return;

        // Get the object that we collided with
        GameObject collidedObject = collision.gameObject;

        // Try to get components from the collided object to figure out what that object is
        PlayerController player = collidedObject.GetComponent<PlayerController>();

        // If collision was with a player
        if (player != null)
        {
            meleeScript.MeleeAttack(collision.collider.gameObject);
            // Bump the enemy back a bit
            navMeshAgent.velocity = new Vector3(navMeshAgent.velocity.normalized.x * -2, 0, navMeshAgent.velocity.normalized.z * -2);
            // Stun the enemy for a bit as an attack cooldown
            Stun(0.5f);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // Don't react to any collisions when we're stunned
        if (stunTimer > 0f)
            return;
            
        // Get the object that we're colliding with
        GameObject collidedObject = collision.gameObject;

        // Try to get components from the colliding object to figure out what that object is
        PlayerController player = collidedObject.GetComponent<PlayerController>();

        // If collision was with a player
        if (player != null)
        {
            // Bump the enemy back a bit
            navMeshAgent.velocity = new Vector3(navMeshAgent.velocity.normalized.x * -2, 0, navMeshAgent.velocity.normalized.z * -2);
            Stun(0.2f);
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

    // Checks if there are any objects from includedLayers between fromPosition and toPosition
    // Returns true if there are no objects, returns false if there are objects or if the line's length exceeds maxDistance
    bool CheckLineOfSight2(Vector3 fromPosition, Vector3 toPosition, string[] includedLayers, float maxDistance = 999999f)
    {
        Vector3 directionToTarget = (toPosition - fromPosition).normalized;
        float distanceToTarget = Vector3.Distance(fromPosition, toPosition);
        
        // If target is too far
        if (distanceToTarget > maxDistance)
        {
            return false;
        }
        
        
        // Create a LayerMask to include the specified collision layers
        LayerMask layerMask = 0;
        foreach (string layer in includedLayers)
        {
            int layerIndex = LayerMask.NameToLayer(layer);
            
            if (layerIndex != -1)
            {
                layerMask |= (1 << layerIndex);
            }
        } 

        // Set the colour of the debug line to gray if the player isn't being chased
        Color debugLineColour = Color.gray;

        // Cast a ray toward the target, check if any objects were hit
        if (Physics.Raycast(fromPosition + Vector3.up, directionToTarget, out RaycastHit hit, distanceToTarget, layerMask))
        {
            if (currentState == EnemyState.Chasing)
            {
                debugLineColour = Color.red;
            }
            Debug.DrawRay(fromPosition, (hit.point - fromPosition), debugLineColour, 0.025f);

            return false;
        }
        else
        {
            if (currentState == EnemyState.Chasing)
            {
                debugLineColour = Color.green;
            }
            Debug.DrawRay(fromPosition, (toPosition - fromPosition), debugLineColour, 0.025f);

            return true;
        }
    }


    public void OnNoiseReceived(Vector3 noisePos)
    {
        if (stunTimer > 0f) return;

        Debug.Log($"Enemy hears noise at {noisePos}");
        noisePosition = noisePos;
        pathfinding.updateDestination(noisePosition);

        if (currentState != EnemyState.Chasing)
        {
            currentState = EnemyState.Investigating;
        }
    }
}