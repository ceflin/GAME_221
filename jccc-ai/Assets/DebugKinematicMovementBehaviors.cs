using UnityEngine;
using System.Collections.Generic;
using System;

public class DebugKinematicMovementBehaviors : MonoBehaviour {

    public KinematicMovementBehaviors behaviors;
    private CharacterMotor associatedMotor;

    private Transform velocityArrow;
    private Transform satisfactionRing;
    private Transform decelerationRing;

    public GameObject breadcrumbTemplate;
    public int maxBreadcrumbs = 50;
    public int breadcrumbsPerSecond = 4;
    private DateTime nextBreadcrumbDrop = DateTime.Now;

    private List<GameObject> breadcrumbs = new List<GameObject>();

	// Use this for initialization
	void Start () {

        velocityArrow = transform.Find("ScalingArrow");
        satisfactionRing = transform.Find("satisfactionRing");
        decelerationRing = transform.Find("decelerationRing");
        associatedMotor = behaviors.GetComponent<CharacterMotor>();

        if (breadcrumbsPerSecond > 10) breadcrumbsPerSecond = 10;
        if (breadcrumbsPerSecond < 0) breadcrumbsPerSecond = 0;

    }
	
	// Update is called once per frame
	void Update () {
        switch (behaviors.currentBehavior)
        {
            case KinematicMovementBehaviors.Behavior.Seek:
                ClearBreadcrumbs();
                // Show radius of satisfcation, if applicable
                decelerationRing.gameObject.SetActive(false);
                if (behaviors.seekUsesRadiusOfSatisfaction)
                {
                    satisfactionRing.gameObject.SetActive(true);
                    satisfactionRing.position = behaviors.target.position;
                    satisfactionRing.localScale = new Vector3(behaviors.radiusOfSatisfaction, behaviors.radiusOfSatisfaction, 1);
                }
                else
                {
                    satisfactionRing.gameObject.SetActive(false);
                }
                break;
            case KinematicMovementBehaviors.Behavior.Arrive:
                ClearBreadcrumbs();
                // Show radii of deceleration and satisfaction
                satisfactionRing.gameObject.SetActive(true);
                satisfactionRing.position = behaviors.target.position;
                satisfactionRing.localScale = new Vector3(behaviors.radiusOfSatisfaction, behaviors.radiusOfSatisfaction, 1);

                decelerationRing.gameObject.SetActive(true);
                decelerationRing.position = behaviors.target.position;
                decelerationRing.localScale = new Vector3(behaviors.radiusOfDeceleration, behaviors.radiusOfDeceleration, 1);
                break;
            case KinematicMovementBehaviors.Behavior.Flee:
                ClearBreadcrumbs();
                satisfactionRing.gameObject.SetActive(false);
                decelerationRing.gameObject.SetActive(false);
                break;
            case KinematicMovementBehaviors.Behavior.Wander:
                satisfactionRing.gameObject.SetActive(false);
                decelerationRing.gameObject.SetActive(false);
                // Show breadcrumbs
                if (DateTime.Now > nextBreadcrumbDrop)
                {
                    GameObject breadcrumb;
                    if (breadcrumbs.Count >= maxBreadcrumbs)
                    {
                        breadcrumb = breadcrumbs[0];
                        breadcrumbs.RemoveAt(0);
                    }
                    else
                    {
                        breadcrumb = GameObject.Instantiate(breadcrumbTemplate);
                        breadcrumb.transform.parent = transform;
                    }
                    breadcrumb.transform.position = behaviors.transform.position + new Vector3(0, 0.1f, 0);
                    breadcrumb.transform.LookAt(breadcrumb.transform.position + behaviors.transform.forward);
                    breadcrumbs.Add(breadcrumb);
                    nextBreadcrumbDrop = DateTime.Now.AddMilliseconds(1000 / breadcrumbsPerSecond);
                }

                break;
        }

        velocityArrow.position = behaviors.transform.position + new Vector3(0, 1, 0);
        velocityArrow.localScale = new Vector3(1, 1, associatedMotor.speed / associatedMotor.maxSpeed);
        velocityArrow.LookAt(velocityArrow.position + behaviors.transform.forward);



    }

    private void ClearBreadcrumbs()
    {
        for (int i = 0; i < breadcrumbs.Count; i++)
        {
            GameObject.Destroy(breadcrumbs[i]);
        }
        breadcrumbs.Clear();
    }

    void OnGUI()
    {
        switch (behaviors.currentBehavior)
        {
            case KinematicMovementBehaviors.Behavior.Seek:
                GUILayout.Label("Seek: Arrow shows direction and scales with speed");
                if (behaviors.seekUsesRadiusOfSatisfaction)
                {
                    GUILayout.Label("Green ring around target indicates radius of satisfcation");
                }
                break;
            case KinematicMovementBehaviors.Behavior.Arrive:
                GUILayout.Label("Arrive: Arrow shows direction and scales with speed");
                GUILayout.Label("Blue outer ring around target indicates radius of deceleration");
                GUILayout.Label("Green inner ring around target indicates radius of satisfcation");
                GUILayout.Label("Speed factor " + behaviors.speedFactor.ToString("0.00"));
                break;
            case KinematicMovementBehaviors.Behavior.Flee:
                GUILayout.Label("Flee: Arrow shows direction and scales with speed");
                break;
            case KinematicMovementBehaviors.Behavior.Wander:
                GUILayout.Label("Wander: Arrow shows direction and scales with speed");
                GUILayout.Label("Breadcrumbs show path walked");
                break;
        }
    }
}
