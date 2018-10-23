using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
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
        if (eyeMount == null) Debug.LogError("Player GameObject Error: No EyeMount child.");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) moveDir += transform.forward;
        if (Input.GetKey(KeyCode.A)) moveDir += -transform.right;
        if (Input.GetKey(KeyCode.S)) moveDir += -transform.forward;
        if (Input.GetKey(KeyCode.D)) moveDir += transform.right;

        characterController.SimpleMove(moveDir.normalized * moveSpeed);
        transform.Rotate(Vector3.up, rotation * (Input.GetAxis("Mouse X") * Time.deltaTime));
        if (eyeMount != null) eyeMount.Rotate(Vector3.right, rotation * (Input.GetAxis("Mouse Y") * Time.deltaTime));
    }
}
