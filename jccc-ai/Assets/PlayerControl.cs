using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 movementDirection = (Input.GetAxis("Vertical") * transform.forward) + (Input.GetAxis("Horizontal") * transform.right);

        GetComponent<CharacterCommands>().Move(movementDirection, 1.0f);


        if (Input.GetAxis("RightHorizontal") != 0)
        {
            print("turning");
            GetComponent<CharacterCommands>().Turn(Input.GetAxis("RightHorizontal"));
        }


	}
}
