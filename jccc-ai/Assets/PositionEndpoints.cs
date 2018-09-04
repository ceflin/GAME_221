using UnityEngine;
using System.Collections;

public class PositionEndpoints : MonoBehaviour {

    TileGraph myTileGraph;

    public KeyCode startPositionKey = KeyCode.S;
    public KeyCode endPositionKey = KeyCode.F;

	// Use this for initialization
	void Start () {

        myTileGraph = GetComponent<TileGraph>();

    }
	
	// Update is called once per frame
	void Update () {
	
        if (Input.GetKeyDown(startPositionKey))
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 1000, 1 << LayerMask.NameToLayer("Surface")))
            {
                transform.Find("Start").position = hitInfo.point;
                Pathfind();
            }
        }

        if (Input.GetKeyDown(endPositionKey))
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 1000, 1 << LayerMask.NameToLayer("Surface")))
            {
                transform.Find("End").position = hitInfo.point;
                Pathfind();
            }
        }

    }

    void Pathfind()
    {
        myTileGraph.Pathfind(transform.Find("Start").position, transform.Find("End").position);
    }
}
