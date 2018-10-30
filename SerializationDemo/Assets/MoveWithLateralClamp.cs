using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithLateralClamp : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float minX = -5f;
    public float maxX = 5f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
            movement += Vector3.left;

        if (Input.GetKey(KeyCode.RightArrow))
            movement += Vector3.right;

        movement.Normalize();

        movement *= (Time.deltaTime * moveSpeed);

        Vector3 newPos = transform.position + movement;
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);

        transform.position = newPos;
    }
}
