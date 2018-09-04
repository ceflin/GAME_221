using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{

    public float radiusOfSatisfaction = 0.5f;
    public float radiusOfDeceleration = 1.5f;

    private Transform target;
    public float speed = 3f;

    private bool isSeeking = false;
    private bool outOfRadiusOfSatisfaction = false;
    

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 targetDir = transform.forward;
        Vector3 targetPos = target.position;
        Vector3 currentPos = transform.position;

        if (isSeeking)
        {
            targetDir = targetPos - currentPos;
            if (outOfRadiusOfSatisfaction)
            {
                this.transform.position = this.transform.position + targetDir * speed * Time.deltaTime;

            }

        }

    }
}
