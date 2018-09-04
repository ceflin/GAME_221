using UnityEngine;
using System.Collections;

public class SteeringSeek : MonoBehaviour {

    public Transform target;

    private CharacterSteeringMotor steeringMotor;

	// Use this for initialization
	void Start () {
        steeringMotor = GetComponent<CharacterSteeringMotor>();
    }
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            steeringMotor.Steer(target.position - transform.position);
        }
    }

    public static void DelegatedSteer(ISteerable steerable, Transform delegatedTarget)
    {
        steerable.Steer(delegatedTarget.position - steerable.Position);
    }

    public static void DelegatedSteer(ISteerable steerable, Vector3 destination)
    {
        steerable.Steer(destination - steerable.Position);
    }
}
