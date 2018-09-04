using UnityEngine;
using System.Collections;

public class WalkForward : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<CharacterMotor>().directionOfMovement = transform.forward;
	}
}
