using UnityEngine;
using System.Collections;

public class KinematicSeek : MonoBehaviour {

    public Vector3 destination;

    private KinematicArrive arrive;

	// Use this for initialization
	void Start () {
        arrive = GetComponent<KinematicArrive>();
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 direction = destination - transform.position;
        direction.y = 0;
        float moveMagnitude = 1;

        if (arrive != null && arrive.enabled)
        {
            float distanceToTarget = Vector3.Distance(transform.position, destination);
            if (distanceToTarget > arrive.radiusOfSatisfaction)
            {
                if (distanceToTarget < arrive.radiusOfDeceleration)
                {
                    moveMagnitude = (distanceToTarget - arrive.radiusOfSatisfaction) / (arrive.radiusOfDeceleration - arrive.radiusOfSatisfaction);
                }
            }
            else
            {
                moveMagnitude = 0;
            }
        }

        GetComponent<CharacterCommands>().Move(direction, moveMagnitude);


        if (GetComponent<LookWhereYouAreGoing>() != null && GetComponent<LookWhereYouAreGoing>().enabled && moveMagnitude > 0)
        {
            GetComponent<CharacterCommands>().Face(direction);
        }



	}
}
