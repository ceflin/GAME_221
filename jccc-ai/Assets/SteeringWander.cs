using UnityEngine;
using System.Collections;

public class SteeringWander : MonoBehaviour {

    public float ringRadius = 1;
    public float ringDistance = 2;
    public float maxDegreeDeltaPerSecond = 360;

    private float currentAnglePosition = 0; // Dead ahead
    private ISteerable myMotor;

    public Vector3 SeekTarget { get; private set; }
    
	// Use this for initialization
	void Start () {
        myMotor = GetComponent<ISteerable>();

    }
	
	// Update is called once per frame
	void Update () {

        currentAnglePosition += Random.Range(-maxDegreeDeltaPerSecond * Time.deltaTime, maxDegreeDeltaPerSecond * Time.deltaTime);
        currentAnglePosition %= 360;

        Vector3 adjustment = Quaternion.AngleAxis(currentAnglePosition, transform.up) * transform.forward;
        adjustment *= ringRadius;

        SeekTarget = transform.position + transform.forward * ringDistance + adjustment;

        SteeringSeek.DelegatedSteer(myMotor, SeekTarget);

	}
}
