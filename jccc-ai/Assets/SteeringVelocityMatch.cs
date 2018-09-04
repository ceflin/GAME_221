using UnityEngine;
using System.Collections;

public class SteeringVelocityMatch : MonoBehaviour {
    
    public Vector3 targetVelocity;

    private CharacterSteeringMotor steeringMotor;

    // Use this for initialization
    void Start()
    {
        steeringMotor = GetComponent<CharacterSteeringMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        steeringMotor.Steer(targetVelocity - steeringMotor.GetComponent<Rigidbody>().velocity);

    }
    

}
