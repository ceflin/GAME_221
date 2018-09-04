using UnityEngine;
using System.Collections;

public class LookForJumps : MonoBehaviour {

    bool jumpTargeted = false;
    Vector3 targetVelocity = Vector3.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hitInfo;
        if (!jumpTargeted)
        {
            if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward, out hitInfo, 4.0f, 1 << LayerMask.NameToLayer("JumpPoint")))
            {
                // Velocity match
                jumpTargeted = true;
                targetVelocity = hitInfo.transform.GetComponent<JumpPoint>().targetVelocity;
                GetComponent<SteeringSeek>().enabled = false;
                GetComponent<SteeringVelocityMatch>().enabled = true;
                GetComponent<SteeringVelocityMatch>().targetVelocity = targetVelocity;
            }
        }
        else
        {
            if (GetComponent<Jump>().JumpExecuted)
            {
                Reset();
            }
        }

    }

    public void Reset()
    {
        jumpTargeted = false;
        GetComponent<SteeringSeek>().enabled = true;
        GetComponent<SteeringVelocityMatch>().enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (jumpTargeted && other.gameObject.layer == LayerMask.NameToLayer("JumpPoint"))
        {
            GetComponent<Jump>().enabled = true;
            GetComponent<Jump>().Reset();
        }
    }

}
