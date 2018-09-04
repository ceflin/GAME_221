using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SteeringSeparation : MonoBehaviour {

    private List<GameObject> others = new List<GameObject>();
    private CharacterSteeringMotor motor;

    public float decayRate = 1.0f;

	// Use this for initialization
	void Start () {
        GameObject[] agents = GameObject.FindGameObjectsWithTag("Agent");
        foreach (GameObject agent in agents)
        {
            if (agent.GetInstanceID() != this.gameObject.GetInstanceID())
                others.Add(agent);
        }
        motor = GetComponent<CharacterSteeringMotor>();
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 steering = Vector3.zero;

        foreach (GameObject other in others)
        {
            Vector3 direction = this.transform.position - other.transform.position;
            float distance = direction.magnitude;
            if (distance > 0)
            {
                direction.Normalize();
                direction *= (decayRate / distance / distance < 1) ? decayRate / distance / distance : 1;
                steering += direction;
            }
        }

        motor.Steer(steering, steering.magnitude);

    }

    public static Vector3 GetSteering(ISteerable steerable, List<Vector3> others, float decayRate)
    {
        Vector3 steering = Vector3.zero;

        foreach (Vector3 other in others)
        {
            Vector3 direction = steerable.Position - other;
            float distance = direction.magnitude;
            if (distance > 0)
            {
                direction.Normalize();
                direction *= (decayRate / distance / distance < 1) ? decayRate / distance / distance : 1;
                steering += direction;
            }
        }
        return steering;
    }
}
