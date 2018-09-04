using UnityEngine;
using System.Collections;

public class AlignToVectorArrows : MonoBehaviour {

    Transform drawSource = null;
    CharacterMotor motor;

    float distanceDebug;

	// Use this for initialization
	void Start () {
        motor = GetComponent<CharacterMotor>();
	}
	
	// Update is called once per frame
	void Update () {
        if (drawSource != null)
        {
            Vector3 toCenter = drawSource.position - transform.position;
            toCenter.y = 0;
            float distanceToCenter = toCenter.magnitude;

            if (toCenter.magnitude > 0)
            {
                toCenter.Normalize();
                transform.LookAt(transform.position + toCenter);

                float moveDistance = motor.speed * Time.deltaTime;
                if (distanceToCenter < moveDistance)
                {
                    transform.position = new Vector3(drawSource.position.x, transform.position.y, drawSource.position.z);
                    transform.LookAt(transform.position + drawSource.forward);
                    drawSource = null;
                } else
                {
                    distanceDebug = moveDistance;
                }
            }
            else
            {
                // Guess we're there..?
                transform.position = new Vector3(drawSource.position.x, transform.position.y, drawSource.position.z);
                transform.LookAt(transform.position + drawSource.forward);
                drawSource = null;
            }



        }

	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("VectorArrow"))
        {
            drawSource = other.transform;
        }
    }

    void OnGUI()
    {
        //GUILayout.Label(centerDebug.ToString("F3"));
        //GUILayout.Label(distanceDebug.ToString());
    }
}
