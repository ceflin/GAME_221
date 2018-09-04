using UnityEngine;
using System.Collections;
using Assets;
using System;
using System.Collections.Generic;
using System.Text;

public class TileGraph : MonoBehaviour {

    public float tileSize = 1.0f;

    private int graphWidth = 20, graphHeight = 20;

    private PathfindingGraph graph;
    private PathfindingNode[] nodes;

    public Color gizmoColor = Color.green;

    private List<PathfindingConnection> pathfindingConnections = new List<PathfindingConnection>();
    List<PathfindingNode> visitedNodes = new List<PathfindingNode>();
   
    public PathfindingStrategy strategy = PathfindingStrategy.Dijkstra;
    private PathfindingStrategy lastStrategy = PathfindingStrategy.Dijkstra;

    // Use this for initialization
    void Start () {
        GenerateGraph();
    }
	
	// Update is called once per frame
	void Update () {
	    if (strategy != lastStrategy)
        {
            lastStrategy = strategy;
            Pathfind();
        }
	}

    void OnGUI()
    {
        if (GUILayout.Button("Generate"))
        {
            GenerateGraph();
        }
    }

    private void GenerateGraph()
    {
        // Generate the graph
        // What's a graph? Nodes and connections.
        graph = new PathfindingGraph();

        // Width is parent X scale; height is parent Y scale
        int graphWidth = Mathf.CeilToInt(transform.localScale.x);
        int graphHeight = Mathf.CeilToInt(transform.localScale.z);

        // Assume that we're in the corner, and using square tiles
        nodes = new PathfindingNode[graphWidth * graphHeight];
        for (int x = 0; x < graphWidth; x++)
        {
            for (int y = 0; y < graphHeight; y++)
            {
                nodes[y * graphWidth + x] = new PathfindingNode(transform.position + new Vector3(tileSize / 2 + x * tileSize, 0.1f, tileSize / 2 + y * tileSize));
            }
        }

        // Check for connections
        for (int i = 0; i < nodes.Length; i++)
        {
            int x = i % graphWidth;
            int y = i / graphWidth;

            CheckConnection(x, y, x - 1, y - 1);
            CheckConnection(x, y, x, y - 1);
            CheckConnection(x, y, x + 1, y - 1);
            CheckConnection(x, y, x + 1, y);
            CheckConnection(x, y, x - 1, y);
            CheckConnection(x, y, x - 1, y + 1);
            CheckConnection(x, y, x, y + 1);
            CheckConnection(x, y, x + 1, y + 1);

        }
    }
    private void Pathfind()
    {
        Pathfind(transform.Find("Start").position, transform.Find("End").position);
    }
    internal void Pathfind(Vector3 start, Vector3 end)
    {
        // 1. Quantize
        Vector3 startLocal = start - transform.position;
        Vector3 endLocal = end - transform.position;

        int startX = Mathf.FloorToInt(startLocal.x / tileSize);
        int startY = Mathf.FloorToInt(startLocal.z / tileSize);
        int endX = Mathf.FloorToInt(endLocal.x / tileSize);
        int endY = Mathf.FloorToInt(endLocal.z / tileSize);
        PathfindingNode startNode = nodes[startY * graphWidth + startX];
        PathfindingNode endNode = nodes[endY * graphWidth + endX];

        Pathfinder pathfinder = PathfinderFactory.Instance.GetPathfinder(graph, strategy);
        
        pathfindingConnections.Clear();
        pathfindingConnections = pathfinder.Pathfind(startNode, endNode, out visitedNodes);

        StringBuilder sb = new StringBuilder();
        //foreach (PathfindingConnection connection in pathfindingConnections)
        //{
        //    sb.AppendLine(string.Format("From {0} to {1}", connection.from.characteristicPoint, connection.to.characteristicPoint));
        //}
        //print(sb.ToString());

    }

    void OnDrawGizmos()
    {
        if (nodes == null) return;


        // Nodes
        Gizmos.color = gizmoColor;
        for (int i = 0; i < nodes.Length; i++)
        {
            Gizmos.DrawSphere(nodes[i].characteristicPoint, tileSize / 5);

            if (graph.connectionsByNode.ContainsKey(nodes[i]))
            {
                foreach (PathfindingConnection connection in graph.connectionsByNode[nodes[i]])
                {
                    Gizmos.DrawLine(connection.from.characteristicPoint, connection.to.characteristicPoint);
                }
            }
        }

        // Visited nodes
        Gizmos.color = Color.red;
        foreach (PathfindingNode visitedNode in visitedNodes)
        {
            Gizmos.DrawSphere(visitedNode.characteristicPoint, tileSize / 4.5f);
        }

        // Pathfinding connections
        Gizmos.color = Color.yellow;
        foreach (PathfindingConnection connection in pathfindingConnections)
        {
            Gizmos.DrawSphere(connection.from.characteristicPoint, tileSize / 4);
            Gizmos.DrawSphere(connection.to.characteristicPoint, tileSize / 4);
        }

    }


    void CheckConnection(int fromX, int fromY, int toX, int toY)
    {
        if (toX >= graphWidth) return;
        if (toX < 0) return;
        if (toY >= graphHeight) return;
        if (toY < 0) return;

        // raycast
        Vector3 from = nodes[fromY * graphWidth + fromX].characteristicPoint;
        Vector3 to = nodes[toY * graphWidth + toX].characteristicPoint;

        float distance = Vector3.Distance(from, to);
        if (!Physics.Raycast(from, to - from, distance))
        {
            // Valid
            PathfindingNode fromNode = nodes[fromY * graphWidth + fromX];
            PathfindingNode toNode = nodes[toY * graphWidth + toX];
            graph.AddDirectional(fromNode, toNode, distance);
        }
    }
    
}
