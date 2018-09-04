using UnityEngine;
using System.Collections;

public class SteeringArrive : MonoBehaviour {

    public Transform target;
    public float outerRadius = 6;
    public float innerRadius = 3;

    private CharacterSteeringMotor steeringMotor;

    // Use this for initialization
    void Start()
    {
        steeringMotor = GetComponent<CharacterSteeringMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        // We want velocity 0 at inner radius-, and velocity full at outerRadius+
        Vector3 currentVelocity = steeringMotor.velocity;
        float targetSpeedFactor = (Mathf.Clamp(Vector3.Distance(transform.position, target.position), innerRadius, outerRadius) - innerRadius) / (outerRadius - innerRadius);

        float targetSpeed = steeringMotor.maxSpeed * targetSpeedFactor;
        Vector3 targetVelocity = target.position - transform.position;
        targetVelocity = targetVelocity.normalized * targetSpeed;


        steeringMotor.Steer(targetVelocity - currentVelocity);
    }

    public static Vector3 GetSteering(ISteerable steerable, Vector3 target, float outerRadius, float innerRadius)
    {
        Vector3 currentVelocity = steerable.Velocity;
        float targetSpeedFactor = (Mathf.Clamp(Vector3.Distance(steerable.Position, target), innerRadius, outerRadius) - innerRadius) / (outerRadius - innerRadius);

        float targetSpeed = steerable.MaxSpeed * targetSpeedFactor;
        Vector3 targetVelocity = target - steerable.Position;
        targetVelocity = targetVelocity.normalized * targetSpeed;

        return targetVelocity - currentVelocity;
    }
}
