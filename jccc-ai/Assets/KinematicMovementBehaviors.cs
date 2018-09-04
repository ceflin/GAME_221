using UnityEngine;
using System.Collections;

public class KinematicMovementBehaviors : MonoBehaviour {

    public Transform target;
    public Behavior currentBehavior = Behavior.Arrive;

    private CharacterMotor motor;

    public bool seekUsesRadiusOfSatisfaction = false;
    public bool seekSetsPositionWithinRadiusOfSatisfaction = false;
    public float radiusOfSatisfaction = 0.5f;
    public float radiusOfDeceleration = 2.0f;

    public float currentTurnRate = 0.0f;
    public float turnChangeRate = 200f;
    public float maxTurnRateMagnitude = 50;

    public float speedFactor = 1.0f;

    public enum Behavior
    {
        Seek,
        Flee,
        Arrive,
        Wander
    }


	// Use this for initialization
	void Start () {
        motor = GetComponent<CharacterMotor>();
	}

	// Update is called once per frame
	void Update () {

        Vector3 targetDirection = transform.forward;
        Vector3 targetPosition = target.position;
        targetPosition.y = 0;
        Vector3 myPosition = transform.position;
        myPosition.y = 0;

        switch (currentBehavior)
        {
            case Behavior.Seek:
                targetDirection = targetPosition - myPosition;
                motor.speed = motor.maxSpeed;

                if (seekUsesRadiusOfSatisfaction && targetDirection.magnitude <= radiusOfSatisfaction)
                {
                    motor.speed = 0;
                    if (seekSetsPositionWithinRadiusOfSatisfaction)
                    {
                        transform.position = targetPosition + new Vector3(0, transform.position.y, 0);
                    }
                }

                break;
            case Behavior.Flee:
                targetDirection = myPosition - targetPosition;
                motor.speed = motor.maxSpeed;
                break;
            case Behavior.Arrive:
                targetDirection = targetPosition - myPosition;
                // But do the arrive thing, too; set the speed based on the motor's max speed

                float effectiveDistance = Mathf.Clamp(Vector3.Distance(targetPosition, myPosition), radiusOfSatisfaction, radiusOfDeceleration);

                speedFactor = (effectiveDistance - radiusOfSatisfaction) / (radiusOfDeceleration - radiusOfSatisfaction);
                motor.speed = motor.maxSpeed * speedFactor;
                break;
            case Behavior.Wander:
                // Ahhhhm it's not really like that here...
                currentTurnRate += UnityEngine.Random.Range(-turnChangeRate, turnChangeRate) * Time.deltaTime;
                currentTurnRate = Mathf.Clamp(currentTurnRate, -maxTurnRateMagnitude, maxTurnRateMagnitude);
                transform.Rotate(Vector3.up, currentTurnRate * Time.deltaTime);
                motor.speed = motor.maxSpeed;
                break;
        }

        if (currentBehavior != Behavior.Wander)
        {
            currentTurnRate = 0.0f;

            targetDirection.y = 0;
            targetDirection.Normalize();

            transform.LookAt(transform.position + targetDirection);
        }

        motor.directionOfMovement = transform.forward;


    }

}
