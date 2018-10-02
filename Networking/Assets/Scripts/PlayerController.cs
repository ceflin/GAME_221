using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotation = 4.0f;
    public float pitch = 3.0f;

    private Transform eyeMount;
    private CharacterController characterController;

    // Use this for initialization
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        eyeMount = transform.Find("EyeMount");
        if (eyeMount == null)
        {
            Debug.LogError("Player GameObject error: No EyeMount child");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) moveDirection += transform.forward;
        if (Input.GetKey(KeyCode.A)) moveDirection += -transform.right;
        if (Input.GetKey(KeyCode.S)) moveDirection += -transform.forward;
        if (Input.GetKey(KeyCode.D)) moveDirection += transform.right;

        characterController.SimpleMove(moveDirection.normalized * moveSpeed);
        transform.Rotate(Vector3.up, rotation * (Input.GetAxis("Mouse X") * Time.deltaTime));
        if (eyeMount != null) eyeMount.Rotate(Vector3.right, rotation * (Input.GetAxis("Mouse Y") * Time.deltaTime));

        //if (!isLocalPlayer)
        //{
        //    return;
        //}

        //var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        //var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        //transform.Rotate(0, x, 0);
        //transform.Translate(0, 0, z);

    }
}
