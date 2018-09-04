using UnityEngine;
using System.Collections;

public class SteeringPursue : MonoBehaviour {

    public Transform target;
    public float maxLookAhead = 2;

    private CharacterSteeringMotor steeringMotor;

    // Use this for initialization
    void Start () {
        steeringMotor = GetComponent<CharacterSteeringMotor>();
	}
	
	// Update is called once per frame
	void Update () {

        // How long do I need to get to my target?
        float rangeToTarget = Vector3.Distance(target.position, transform.position);
        float timeToTarget = rangeToTarget / steeringMotor.maxSpeed;        // d / d / s = s

        if (timeToTarget > maxLookAhead) timeToTarget = maxLookAhead;
        // Where will the target be in that time?

        Vector3 targetVelocity = target.GetComponent<Rigidbody>().velocity;

        Vector3 destination = target.position + targetVelocity * timeToTarget;

        SteeringSeek.DelegatedSteer(steeringMotor, destination);

	}
}
