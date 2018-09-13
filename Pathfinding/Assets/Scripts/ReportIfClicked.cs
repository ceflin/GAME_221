using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportIfClicked : MonoBehaviour
{
    public TileGeneration generatedTiles;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        mouseInput();
    }

    private void mouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Node")
            {
                Node myNode = generatedTiles.tilesToNodes[this.gameObject];
                print(transform.position.ToString() + myNode.pos);
            }
        }
    }
}
