using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPatrolRoute : MonoBehaviour
{
    // Whether this object is following its normal patrol route
    private bool isPatrolling = true;
    // The specific route this object is following
    public int routeID;
    // All the nodes that exist in this path. The array is populated during Start()
    private PatrolNode[] routeNodes;
    // The node that the object is meant to be going towards
    private PatrolNode destinationNode;

    void Start()
    {
        // Find all the nodes in our route and save them for later use
        routeNodes = findAllNodesInRoute(routeID);
        // Find the first node to path to
        //destinationNode = findNextNode(-1);

        
    }



    // Returns a list of all the patrol node objects in a given route
    PatrolNode[] findAllNodesInRoute(int route)
    {
        // Find all objects of type PatrolNode
        PatrolNode[] objects = GameObject.FindObjectsOfType<PatrolNode>();
        // Filter objects based on routeID
        List<PatrolNode> matchingNodes = new List<PatrolNode>();
        
        foreach (PatrolNode obj in objects)
        {
            if (obj.routeID == route)
            {
                matchingNodes.Add(obj);
            }
        }

        return matchingNodes.ToArray();
    }

    // Returns the node from routeNodes that comes after fromNodeID
    // TODO this whole function is missing a lot of checks.
    // What if we accidentally make a route with multiple nodes that share the same nodeID? Or routes that skip a nodeID?
    PatrolNode findNextNode(int fromNodeID = -100)
    {
        // Workaround for the C# compiler not allowing me to put a variable in the function's default parameters
        if (fromNodeID == -100)
        {
            fromNodeID = destinationNode.nodeID;
        }

        // Initialize variable to store potential first node
        PatrolNode firstNode = null; 
        
        // Iterate through all nodes in the route
        foreach (PatrolNode node in routeNodes)
        {
            // Check if this is the next sequential node
            if (node.nodeID == fromNodeID + 1)
            {
                return node;  // Found the next node, exit early
            }
            // Store the first node we encounter (has a nodeID of 0)
            else if (node.nodeID == 0)
            {
                firstNode = node;
            }
        }
        // If we get here, no next sequential node was found
        return firstNode;
    }
}