using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolNode : MonoBehaviour
{
    // Which route this node is a part of
    public int routeID;
    // Which node this is, sequentially
    public int nodeID;

    /*
    A patrol "route" is made up of these patrol nodes.

    All routes must:
    - Have at least two nodes
    - Have a node with a nodeID of 0
    - Not have any gaps in their nodeIDs (e.g. 0, 1, 2, 4)
    
    All nodes in a route must:
    - Have unique nodeIDs
    */
}
