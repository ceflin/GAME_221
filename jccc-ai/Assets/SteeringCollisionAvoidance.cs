using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SteeringCollisionAvoidance : MonoBehaviour {

    private List<GameObject> others = new List<GameObject>();
    private CharacterSteeringMotor motor;

    // Use this for initialization
    void Start()
    {
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
	


	}
}
