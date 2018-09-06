using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{

    public float radiusOfSatisfaction = 0.5f;
    public float speed = 3f;

    private Vector3 target;
    //private Transform target;
    private CharacterController characterController;
    private bool isSeeking = false;

    // Use this for initialization
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 targetDir = transform.forward;
        //Vector3 targetPos = target.position;
        Vector3 currentPos = transform.position;

        if (isSeeking)
        {
            targetDir = target - currentPos;
            transform.LookAt(target);
            characterController.Move(targetDir.normalized * speed * Time.deltaTime);

            if (Vector3.Distance(target, currentPos) <= radiusOfSatisfaction)
            {
                isSeeking = false;
            }

        }

    }

    public void Seek(Vector3 position)
    {
        target = position;
        isSeeking = true;
    }
}
