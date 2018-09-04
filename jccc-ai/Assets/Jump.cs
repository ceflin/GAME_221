using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {

    private Animator myAnimator;
    private bool landed = true;
    private Rigidbody myRigidbody;
    private Vector3 forwardMotion;
    private CharacterSteeringMotor motor;

    public bool JumpExecuted { get; private set; }
	// Use this for initialization

    void Awake()
    {
        enabled = false;
    }

	void Start () {
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody>();
        motor = GetComponent<CharacterSteeringMotor>();
    }
	
	// Update is called once per frame
	void Update () {

        if (!landed)
        {
            if (myRigidbody.velocity.y < 0 && Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), Vector3.down, 0.15f, 1 << LayerMask.NameToLayer("Surface")))
            {
                landed = true;
                myAnimator.SetTrigger("OnGrounded");
                motor.enabled = true;
                GetComponent<CharacterSteeringMotor>().enabled = true;
                enabled = false;
            }
        }
        else
        {
            // Trigger the jump 
            GetComponent<CharacterSteeringMotor>().enabled = false;
            myAnimator.SetTrigger("OnJump");
            landed = false;
            forwardMotion = myRigidbody.velocity;
            forwardMotion.y = 0;
            motor.enabled = false;
            JumpExecuted = true;
        }

    }

    public void Reset()
    {
        landed = true;
        JumpExecuted = false;
    }

    void FixedUpdate()
    {
        if (!landed)
        {
            myRigidbody.velocity = new Vector3(Vector3.Dot(forwardMotion, Vector3.right), myRigidbody.velocity.y, Vector3.Dot(forwardMotion, Vector3.forward));
        }
    }

}
