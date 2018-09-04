using UnityEngine;
using System.Collections;

public class CharacterMotor : MonoBehaviour {

    public float maxSpeed = 3;
    public float speed = 3;

    public Vector3 directionOfMovement;

    protected Vector3 gravity = Vector3.zero;
    protected Rigidbody characterBody;

    public bool isGrounded;

    public Animator myAnimator;

    public bool useAnimationDrivenMotion = true;

    protected void Awake()
    {
        characterBody = GetComponent<Rigidbody>();
    }

	// Use this for initialization
	void Start () {


	}

    void Update()
    {
        // Instead of telling the rigidbody how it's moving,
        // play the animations and let root motion guide it.
        // Should still snag on physics.
        //myAnimator.SetFloat("Forward", 1.0f);
        //myAnimator.SetFloat("Right", 0.0f);

        // If motion is separate from orientation, then what we pass for forward and right needs to reflect that.

        // Difference between directionOfMovement and transform.forward.
        if (useAnimationDrivenMotion)
        {
            if (directionOfMovement != Vector3.zero)
            {
                float angle = Vector3.Angle(directionOfMovement, transform.forward);
                directionOfMovement.Normalize();
                directionOfMovement *= (speed / maxSpeed);
                myAnimator.SetFloat("Forward", Vector3.Dot(directionOfMovement, transform.forward));
                myAnimator.SetFloat("Right", Vector3.Dot(directionOfMovement, transform.right));
            }
            else
            {
                myAnimator.SetFloat("Forward", 0f);
                myAnimator.SetFloat("Right", 0f);
            }

        }
    }

    void FixedUpdate()
    {
        if (!useAnimationDrivenMotion)
        {
            if (speed > maxSpeed) speed = maxSpeed;
            Vector3 movement = directionOfMovement.normalized * speed;
            characterBody.velocity = new Vector3(movement.x, characterBody.velocity.y, movement.z);
        }

    }

}
