using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra
{

    public static List<Vector3> Pathfind(TileGraph graph, Node fromNode, Node toNode)
    {
        List<Vector3> waypoints = new List<Vector3>();

        //TODO : implement Dijkstra
        List<PathfindingNode> openList = new List<PathfindingNode>();
        List<PathfindingNode> closedList = new List<PathfindingNode>();

        Dictionary<Node, PathfindingNode> pathfindingNodes = new Dictionary<Node, PathfindingNode>();

        pathfindingNodes.Add(fromNode, new PathfindingNode(fromNode));
        openList.Add(pathfindingNodes[fromNode]);

        while (openList.Count > 0 && !DoesListContainNode(toNode, closedList))
        {
            //TODO find connections from the lowest cost so far point to all connected points
            // How do I know what the lowest cost so far is?
            openList.Sort();
            PathfindingNode smallestCostSoFar = openList[0];


            // How do we get connections?
            foreach (Node connectedNode in smallestCostSoFar.graphNode.connections.Keys)
            {
                if (!DoesListContainNode(connectedNode, closedList))
                {
                    if (!DoesListContainNode(connectedNode, openList))
                    {
                        float costToConnectedNode = smallestCostSoFar.costSoFar + smallestCostSoFar.graphNode.connections[connectedNode];
                        PathfindingNode predecessor = smallestCostSoFar;

                        pathfindingNodes.Add(connectedNode, new PathfindingNode(connectedNode, costToConnectedNode, predecessor));
                        openList.Add(pathfindingNodes[connectedNode]);
                    }
                    else
                    {
                        // Is my connection from the current processing node faster than the existing connectino to this node?
                        // If so, update it.

                        float currentCostToTarget = pathfindingNodes[connectedNode].costSoFar;
                        float costToTargetThroughCurrentNode = smallestCostSoFar.costSoFar + smallestCostSoFar.graphNode.connections[connectedNode];

                        if(costToTargetThroughCurrentNode < currentCostToTarget)
                        {
                            pathfindingNodes[connectedNode].costSoFar = costToTargetThroughCurrentNode;
                            pathfindingNodes[connectedNode].predecessor = smallestCostSoFar;
                        }
                    }
                }
            }

            closedList.Add(smallestCostSoFar);
            openList.Remove(smallestCostSoFar);

        }//end of while loop - pathfinding complete
        
        //TODO fill out waypoints

        //Destination node is on closed list
        //All of its predecessors are on the closed list

        for (PathfindingNode waypoint = pathfindingNodes[toNode]; waypoint != null; waypoint = waypoint.predecessor)
        {
            waypoints.Add(waypoint.graphNode.pos);
        }

        waypoints.Reverse();


        return waypoints;
    }

    private static bool DoesListContainNode(Node searchedNode, List<PathfindingNode> pathfindingNodeList)
    {
        foreach (PathfindingNode pathfindingNode in pathfindingNodeList)
            if (pathfindingNode.graphNode == searchedNode)
                return true;
        return false;
    }

}




public class PathfindingNode : System.IComparable<PathfindingNode>
{
    public Node graphNode;
    public float costSoFar;

    //TODO predecessor
    public PathfindingNode predecessor;

    public int CompareTo(PathfindingNode other)
    {
        return costSoFar.CompareTo(other.costSoFar);
    }

    public PathfindingNode(Node graphNode, float costSoFar, PathfindingNode predecessor)
    {
        this.graphNode = graphNode;
        this.costSoFar = costSoFar;
        this.predecessor = predecessor;
    }

    public PathfindingNode(Node originalNode)
    {
        this.graphNode = originalNode;
        costSoFar = 0.0f;
        predecessor = null;
    }
}


public class TileGraph
{
    public List<Vector3> points = new List<Vector3>();
}

public class Node
{
    public Node(Vector3 pos)
    {
        this.pos = pos;
    }

    public Vector3 pos;

    //Slight limitation, only one connection is possible to each other node
    public Dictionary<Node, float> connections = new Dictionary<Node, float>();
}
