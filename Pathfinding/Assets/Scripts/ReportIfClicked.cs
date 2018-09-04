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
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                print("My location is " + transform.position.ToString("F1") + ", my node is...?");

                Node myNode = generatedTiles.tilesToNodes[this.gameObject];
            }
        }
    }
}
