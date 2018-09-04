using UnityEngine;
using System.Collections;

public class SteeringFace : MonoBehaviour {

    public Transform target;

    private CharacterSteeringMotor myMotor;

    // Use this for initialization
    void Start () {
        myMotor = GetComponent<CharacterSteeringMotor>();
	}
	
	// Update is called once per frame
	void Update () {
	
        if (target != null)
        {
            SteeringAlign.DelegatedSteer(myMotor, target.position - transform.position);
        }

	}
}
