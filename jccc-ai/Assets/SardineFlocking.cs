using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SardineFlocking : MonoBehaviour {

    private SchoolControl schoolControl;
    private ISteerable steerable;
    private List<Vector3> neighbors = new List<Vector3>();

	// Use this for initialization
	void Start () {
        schoolControl = transform.parent.GetComponent<SchoolControl>();
        steerable = GetComponent<ISteerable>();
        GetComponent<Animator>().SetFloat("AnimationOffset", Random.Range(0f, 1f));
    }
	
	// Update is called once per frame
	void Update () {

        // THOUGHT: May want to calculate influence less frequently than every frame?
        neighbors.Clear();
        Vector3 averagePosition = Vector3.zero, averageVelocity = Vector3.zero;
        foreach (ISteerable member in schoolControl.members)
        {
            neighbors.Add(member.Position);
            if ((member.Position - transform.position).sqrMagnitude > schoolControl.SqrNeighborhoodRange)
            {
                averagePosition += member.Position;
                averageVelocity += member.Velocity;
            }
        }
        averagePosition /= neighbors.Count;
        averageVelocity /= neighbors.Count;

        // Arrive: average position
        Vector3 arriveOutput = SteeringArrive.GetSteering(steerable, averagePosition, schoolControl.arriveOuterRadius, schoolControl.arriveInnerRadius);

        // Velocity match: average velocity - that's easy enough
        Vector3 velocityMatchOutput = averageVelocity;

        // Separate: Undelegated...
        Vector3 separationOutput = SteeringSeparation.GetSteering(steerable, neighbors, schoolControl.separationDecay);

        arriveOutput *= schoolControl.cohesionWeight;
        velocityMatchOutput *= schoolControl.velocityMatchWeight;
        separationOutput *= schoolControl.separationWeight;

        Vector3 flocking = arriveOutput + velocityMatchOutput + separationOutput;

        steerable.Steer(flocking, flocking.magnitude);

    }
}
