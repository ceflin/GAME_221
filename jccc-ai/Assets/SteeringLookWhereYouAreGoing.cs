using UnityEngine;
using System.Collections;

public class SteeringLookWhereYouAreGoing : MonoBehaviour {

    private ISteerable mySteerable;

	// Use this for initialization
	void Start () {
        mySteerable = GetComponent<ISteerable>();

    }
	
	// Update is called once per frame
	void Update () {

        SteeringAlign.DelegatedSteer(mySteerable, mySteerable.Velocity);

    }
}
