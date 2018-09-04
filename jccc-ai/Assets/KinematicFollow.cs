using UnityEngine;
using System.Collections.Generic;

public class KinematicFollow : MonoBehaviour {

    public Transform followTarget;
    public float activationDistance = 2.0f;
    private float remainingActivationDistance = 0.0f;
    public float radiusOfSatisfaction = 0.01f;
    //private float targetTravelDistance = 0.0f;

    //private bool activated = false;
    private Queue<Vector3> waypoints = new Queue<Vector3>();

    private Vector3 targetLastPosition;

	// Use this for initialization
	void Start () {

        waypoints.Enqueue(followTarget.position);
        targetLastPosition = followTarget.position;
        remainingActivationDistance = activationDistance;
    }

    // Update is called once per frame
    void Update () {

        if (followTarget.position != targetLastPosition)
        {
            waypoints.Enqueue(followTarget.position);
            // How far did they move?
            float targetMoved = Vector3.Distance(targetLastPosition, followTarget.position);

            // Move that far
            float travelRemaining = targetMoved;
            while (travelRemaining > 0)
            {
                if (remainingActivationDistance > 0)
                {
                    if (remainingActivationDistance < travelRemaining)
                    {
                        travelRemaining -= remainingActivationDistance;
                        remainingActivationDistance = 0;
                    }
                    else
                    {
                        remainingActivationDistance -= travelRemaining;
                        travelRemaining = 0;
                    }
                }
                else
                {
                    // Move toward waypoints until we run out of distance
                    Vector3 nextWaypoint = waypoints.Peek();
                    float distance = Vector3.Distance(transform.position, nextWaypoint);
                    if (distance <= travelRemaining)
                    {
                        // We can reach the next waypoint, do so and continue
                        travelRemaining -= distance;
                        transform.position = waypoints.Dequeue();
                    }
                    else
                    {
                        // Move along the track however much we have left
                        Vector3 direction = nextWaypoint - transform.position;
                        transform.position += direction.normalized * travelRemaining;
                        travelRemaining = 0;
                    }
                }
            }

            Vector3 next = waypoints.Peek();
            next.y = transform.position.y;
            transform.LookAt(next, Vector3.up);

            targetLastPosition = followTarget.position;
        }



    }
}
