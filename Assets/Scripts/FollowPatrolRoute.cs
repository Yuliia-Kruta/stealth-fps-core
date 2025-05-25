using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPatrolRoute : MonoBehaviour
{
    // Whether this object is following its normal patrol route (currently unused)
    //private bool isPatrolling = true;
    // The specific route this object is following
    public int routeID;
    // All the nodes that exist in this path. The array is populated during Start()
    private PatrolNode[] routeNodes;
    // The node that the object is meant to be going towards
    public PatrolNode destinationNode;


    void Start()
    {
        // Find all the nodes in our route and save them for later use
        routeNodes = findAllNodesInRoute(routeID);

        // Set the destination to the first node in the route
        destinationNode = findNodeByID(0);
    }


    // Returns a list of all the patrol node objects in a given route
    PatrolNode[] findAllNodesInRoute(int routeID)
    {
        // Find all objects of type PatrolNode
        PatrolNode[] objects = GameObject.FindObjectsOfType<PatrolNode>();
        // Filter objects based on routeID
        List<PatrolNode> matchingNodes = new List<PatrolNode>();
        
        foreach (PatrolNode obj in objects)
        {
            if (obj.routeID == routeID)
            {
                matchingNodes.Add(obj);
            }
        }

        return matchingNodes.ToArray();
    }


    // Returns the first node from routeNodes with the given nodeID
    public PatrolNode findNodeByID(int nodeID)
    {
        // Initialize variable to store potential first node
        PatrolNode firstNode = null; 
        
        // Iterate through all nodes in the route
        foreach (PatrolNode node in routeNodes)
        {
            // Check if this is the right node
            if (node.nodeID == nodeID)
            {
                return node; // Found the next node, exit early
            }
            // Store the route's first node (has a nodeID of 0)
            else if (node.nodeID == 0)
            {
                firstNode = node;
            }
        }

        // If we get here, no node was found. Return the first node of the route
        return firstNode;
    }
}