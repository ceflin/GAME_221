using UnityEngine;
using System.Collections;

public class ClickToSetTarget : MonoBehaviour {

    public Transform target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if (Input.GetMouseButtonDown(0) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            RaycastHit info;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out info, 1000, 1 << LayerMask.NameToLayer("Surface")))
            {
                target.position = info.point;
            }
        }

	}
}
