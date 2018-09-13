using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGeneration : MonoBehaviour {

    public int gridWidth = 0;
    public int gridHeight = 0;

    public GameObject tileTemplate;

    public Dictionary<GameObject, Node> tilesToNodes = new Dictionary<GameObject, Node>();

    void Start()
    {

        Dictionary<Vector3, Node> nodesByPosition = new Dictionary<Vector3, Node>();

        for (int x = 0; x <gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                GameObject newTile = GameObject.Instantiate(tileTemplate);
                newTile.transform.position = new Vector3(x, 0, z);

                Node tileNode = new Node(newTile.transform.position);
                nodesByPosition.Add(tileNode.pos, tileNode);

                tilesToNodes.Add(newTile, tileNode);
                //newTile.GetComponent<NodeBinding>().node = tileNode;

                //newTile.GetComponent<ReportIfClicked>().generatedTiles = this;
                
            }
        }

        nodesByPosition.
        
        foreach (Vector3 nodePosition in nodesByPosition.Keys)
        {
            Node currentNode = nodesByPosition[nodePosition];
            Dictionary<Node, float> weightedConnections = currentNode.connections;

            Node right = LookupNode(nodesByPosition, nodePosition + Vector3.right);
            if (right != null)
                weightedConnections.Add(right, 1);

            Node left = LookupNode(nodesByPosition, nodePosition + Vector3.left);
            if (left != null)
                weightedConnections.Add(left, 1);

            Node up = LookupNode(nodesByPosition, nodePosition + Vector3.up);
            if (up != null)
                weightedConnections.Add(up, 1);

            Node down = LookupNode(nodesByPosition, nodePosition + Vector3.down);
            if (down != null)
                weightedConnections.Add(down, 1);
            print(currentNode.pos);
        }
        //Create connections after generation is complete



    }

    Node LookupNode(Dictionary<Vector3, Node> nodes, Vector3 lookup)
    {
        if (!nodes.ContainsKey(lookup))
        {
            return null;
        }
        return nodes[lookup];
    }

    void Update()
    {

    }
}
