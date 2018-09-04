using UnityEngine;
using System.Collections;

public class SteeringPathFollowing : MonoBehaviour {

    public Path path;
    public float projectionDistance = 5;
    private CharacterSteeringMotor motor;

    public Vector3 SeekTarget { get; private set; }

	// Use this for initialization
	void Start () {
        motor = GetComponent<CharacterSteeringMotor>();

    }
	
	// Update is called once per frame
	void Update () {

        Vector3 target = path.ProjectDownPath(transform.position, projectionDistance);
        target.y = transform.position.y;
        SteeringSeek.DelegatedSteer(motor, target);
        SeekTarget = target;
	}
}
