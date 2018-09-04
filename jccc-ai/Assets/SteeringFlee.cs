using UnityEngine;
using System.Collections;

public class SteeringFlee : MonoBehaviour {

    public Transform target;

    private CharacterSteeringMotor steeringMotor;

    // Use this for initialization
    void Start()
    {
        steeringMotor = GetComponent<CharacterSteeringMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            steeringMotor.Steer(transform.position - target.position);
        }
    }

    public static void DelegatedSteer(CharacterSteeringMotor motor, Transform delegatedTarget)
    {
        motor.Steer(motor.transform.position - delegatedTarget.position);
    }

    public static void DelegatedSteer(CharacterSteeringMotor motor, Vector3 destination)
    {
        motor.Steer(motor.transform.position - destination);
    }
}
