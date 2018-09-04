using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNodesOnSurface : MonoBehaviour
{

    public float tileSize = 1.0f;

    public GameObject spherePlaceHolder;

    // Use this for initialization
    void Start()
    {

        Dictionary<Vector3, Node> nodesByPosition = new Dictionary<Vector3, Node>();

        int gridWidth = Mathf.RoundToInt(transform.localScale.x);
        int gridHeight = Mathf.RoundToInt(transform.localScale.y);

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
               
                Vector3 nodePosition = new Vector3(
                    x,
                    transform.position.y,
                    z);

                Node tileNode = new Node(nodePosition);
                nodesByPosition.Add(tileNode.pos, tileNode);
            }
        }
        foreach (Vector3 nodePosition in nodesByPosition.Keys)
        {
            Node currentNode = nodesByPosition[nodePosition];
            Dictionary<Node, float> weightedConnections = currentNode.connections;

            Node right = LookupNode(nodesByPosition, nodePosition + Vector3.right);
            if (right != null && noObstruction(currentNode, right))
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

        }
    }

    bool noObstruction(Node currentNode, Node targetNode)
    {
        if (!Physics.Raycast (currentNode.pos, targetNode.pos - currentNode.pos))
        {
            return true;
        }
        print("Obstruction between");
        return false;
    }

    Node LookupNode(Dictionary<Vector3, Node> nodes, Vector3 lookup)
    {
        if (!nodes.ContainsKey(lookup))
        {
            return null;
        }
        return nodes[lookup];
    }


    // Update is called once per frame
    void Update()
    {

    }
}
