using UnityEngine;
using System.Collections;

public class CharacterCommands : MonoBehaviour {

    public float movementSpeed = 5.0f;
    public float rotationRate = Mathf.PI * 2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Face(Vector3 direction)
    {
        Vector3 newFace = Vector3.RotateTowards(transform.forward, direction, rotationRate * Time.deltaTime, 0);
        transform.LookAt(transform.position + newFace);
    }

    public void Turn(float rotationMultiplier)
    {
        Quaternion rotation = Quaternion.AngleAxis(rotationRate * Time.deltaTime * rotationMultiplier, Vector3.up);
        Vector3 newDirection = rotation * transform.forward;

        transform.LookAt(transform.position + newDirection);
    }

    public void Move(Vector3 direction, float speedMultiplier)
    {
        Vector3 directionNormalized = direction.normalized;
        Vector3 movement = directionNormalized * (speedMultiplier * movementSpeed * Time.deltaTime);


        transform.position += movement;

    }
}
