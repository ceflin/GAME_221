using UnityEngine;
using System.Collections.Generic;
using System;

public class DebugSteeringMovementBehaviors : MonoBehaviour {

    public CharacterSteeringMotor associatedMotor;

    private Transform velocityArrow;
    private Transform accelerationArrow;

    private Transform satisfactionRing;
    private Transform decelerationRing;
    private Transform ring;

    public GameObject breadcrumbTemplate;
    public int maxBreadcrumbs = 50;
    public int breadcrumbsPerSecond = 4;
    private DateTime nextBreadcrumbDrop = DateTime.Now;

    private List<GameObject> breadcrumbs = new List<GameObject>();
    private SteeringPathFollowing pathFollowingBehavior;
    private SteeringWander steeringWanderBehavior;
    private Transform marker;

    public GameObject scalingLineTemplate;
    public float breadcrumbOffset = 0.05f;

    // Use this for initialization
    void Start () {

        velocityArrow = transform.Find("velocityArrow");
        accelerationArrow = transform.Find("accelerationArrow");
        satisfactionRing = transform.Find("satisfactionRing");
        decelerationRing = transform.Find("decelerationRing");
        marker = transform.Find("marker");
        ring = transform.Find("ring");

        pathFollowingBehavior = associatedMotor.GetComponent<SteeringPathFollowing>();
        if (pathFollowingBehavior != null && pathFollowingBehavior.isActiveAndEnabled)
        {
            Path path = pathFollowingBehavior.path;
            foreach (VectorSegment seg in path.Segments)
            {
                GameObject pathArrow = GameObject.Instantiate(scalingLineTemplate) as GameObject;
                pathArrow.transform.position = new Vector3(seg.p1.x, associatedMotor.transform.position.y + 0.05f, seg.p1.z);
                pathArrow.transform.forward = seg.direction;
                pathArrow.transform.localScale = new Vector3(1, 1, seg.length / 2);
                pathArrow.name = "Path Arrow";
                pathArrow.transform.parent = this.transform;
            }
        }

        steeringWanderBehavior = associatedMotor.GetComponent<SteeringWander>();


        if (breadcrumbsPerSecond > 10) breadcrumbsPerSecond = 10;
        if (breadcrumbsPerSecond < 0) breadcrumbsPerSecond = 0;

    }

    // Update is called once per frame
    void Update()
    {
        velocityArrow.position = associatedMotor.transform.position + new Vector3(0, 1, 0);
        velocityArrow.localScale = new Vector3(1, 1, associatedMotor.velocity.magnitude / associatedMotor.maxSpeed);
        velocityArrow.LookAt(velocityArrow.position + associatedMotor.velocity);

        accelerationArrow.position = associatedMotor.transform.position + new Vector3(0, 0.5f, 0);
        accelerationArrow.localScale = new Vector3(1, 1, 0.5f + associatedMotor.FrameAcceleration.magnitude);
        accelerationArrow.LookAt(accelerationArrow.position + associatedMotor.FrameAcceleration);

        if (nextBreadcrumbDrop < DateTime.Now)
        {
            nextBreadcrumbDrop = nextBreadcrumbDrop.AddMilliseconds(1000f / (float)breadcrumbsPerSecond);
            GameObject breadcrumb = GameObject.Instantiate(breadcrumbTemplate);
            breadcrumb.name = "breadcrumb";
            breadcrumb.transform.position = new Vector3(associatedMotor.transform.position.x, associatedMotor.transform.position.y + breadcrumbOffset, associatedMotor.transform.position.z);
            breadcrumb.transform.LookAt(breadcrumb.transform.position + associatedMotor.transform.forward);
            breadcrumbs.Add(breadcrumb);
            breadcrumb.transform.parent = transform;
            if (breadcrumbs.Count == maxBreadcrumbs)
            {
                //print("Destroying breadcrumb");
                GameObject.Destroy(breadcrumbs[0]);
                breadcrumbs.RemoveAt(0);
                
            }
        }

        if (marker != null) marker.gameObject.SetActive(false);
        if (ring != null) ring.gameObject.SetActive(false);
        if (pathFollowingBehavior != null && pathFollowingBehavior.isActiveAndEnabled)
        {
            // Put something at the seek target
            marker.position = pathFollowingBehavior.SeekTarget;
            marker.gameObject.SetActive(true);
        }

        if (steeringWanderBehavior != null && steeringWanderBehavior.isActiveAndEnabled)
        {
            // Collect debugs for wander? What do we need? A circle and the seek target.
            // Well we have the circle data; next we need the seek target.
            marker.position = steeringWanderBehavior.SeekTarget;
            ring.position = steeringWanderBehavior.transform.position + steeringWanderBehavior.transform.forward * steeringWanderBehavior.ringDistance;
            ring.localScale = new Vector3(steeringWanderBehavior.ringRadius, 1, steeringWanderBehavior.ringRadius);
            marker.gameObject.SetActive(true);
            ring.gameObject.SetActive(true);
        }
    }

}
