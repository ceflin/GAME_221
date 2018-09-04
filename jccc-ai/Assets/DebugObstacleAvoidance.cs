using UnityEngine;
using System.Collections;

public class DebugObstacleAvoidance : MonoBehaviour {

    public SteeringObstacleAvoidance avoid;

    private Transform marker;

	// Use this for initialization
	void Start () {
        marker = transform.Find("marker");
	}
	
	// Update is called once per frame
	void Update () {
        marker.gameObject.SetActive(false);
        if (avoid.isActiveAndEnabled)
        {
            marker.gameObject.SetActive(true);
            marker.position = avoid.SeekTarget;
        }
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Transform mover = avoid.transform;
        Vector3 projection = mover.forward * avoid.primaryDistance;
        Vector3 r, l, w, moverPos = mover.position;
        float whiskerAngle = avoid.whiskerAngle;
        switch (avoid.myConfiguration)
        {
            case SteeringObstacleAvoidance.WhiskerConfiguration.Parallel:
                r = moverPos + mover.right * 0.5f;
                l = moverPos + mover.right * -0.5f;
                Gizmos.DrawLine(l, l + projection);
                Gizmos.DrawLine(r, r + projection);
                break;
            case SteeringObstacleAvoidance.WhiskerConfiguration.SingleOnly:
                Gizmos.DrawLine(moverPos, moverPos + projection);
                break;
            case SteeringObstacleAvoidance.WhiskerConfiguration.SingleWithShortWhiskers:
                Gizmos.DrawLine(moverPos, moverPos + projection);
                w = projection * avoid.secondaryRatio;
                l = Quaternion.AngleAxis(whiskerAngle, Vector3.up) * w;
                r = Quaternion.AngleAxis(-whiskerAngle, Vector3.up) * w;
                Gizmos.DrawLine(moverPos, moverPos + l);
                Gizmos.DrawLine(moverPos, moverPos + r);
                break;
            case SteeringObstacleAvoidance.WhiskerConfiguration.WhiskersOnly:
                w = projection;
                l = Quaternion.AngleAxis(whiskerAngle, Vector3.up) * w;
                r = Quaternion.AngleAxis(-whiskerAngle, Vector3.up) * w;
                Gizmos.DrawLine(moverPos, moverPos + l);
                Gizmos.DrawLine(moverPos, moverPos + r);
                break;
        }
        Gizmos.color = Color.blue;
        if (avoid != null && avoid.Hits != null)
        {
            foreach (Vector3 hit in avoid.Hits)
            {
                Gizmos.DrawSphere(hit, 0.25f);
            }
        }
    }
}
