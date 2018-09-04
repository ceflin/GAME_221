using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SteeringObstacleAvoidance : MonoBehaviour {

    public Transform seek;
    public LayerMask obstacleLayers;

    private CharacterSteeringMotor myMotor;

    public enum WhiskerConfiguration
    {
        SingleOnly,
        WhiskersOnly,
        SingleWithShortWhiskers,
        Parallel
    }

    public WhiskerConfiguration myConfiguration;
    public float primaryDistance = 10.0f;
    public float whiskerAngle = 15.0f;
    public float secondaryRatio = 0.33f;
    public float repulsionDistance = 4;

    public List<Vector3> Hits { get; private set; }
    public Vector3 SeekTarget { get; private set; }

    // Use this for initialization
    void Start () {
        myMotor = GetComponent<CharacterSteeringMotor>();
        Hits = new List<Vector3>();
        
    }

    // Update is called once per frame
    void Update () {
        Vector3 projection = transform.forward * primaryDistance;
        Vector3 r, l, w;
        Vector3 pos = transform.position;
        SeekTarget = seek.position;
        Hits.Clear();

        RaycastHit hitInfo;

        switch (myConfiguration)
        {
            case SteeringObstacleAvoidance.WhiskerConfiguration.Parallel:
                r = pos + transform.right * 0.5f;
                l = pos + transform.right * -0.5f;

                // Perform raycasts to avoid obstacles.1
                if (Physics.Raycast(l, transform.forward, out hitInfo, primaryDistance, obstacleLayers))
                    Hits.Add(GetRepulsionPoint(hitInfo));
                if (Physics.Raycast(r, transform.forward, out hitInfo, primaryDistance, obstacleLayers))
                    Hits.Add(GetRepulsionPoint(hitInfo));
                
                break;
            case SteeringObstacleAvoidance.WhiskerConfiguration.SingleOnly:
                if (Physics.Raycast(pos, transform.forward, out hitInfo, primaryDistance, obstacleLayers))
                    Hits.Add(GetRepulsionPoint(hitInfo));
                break;
            case SteeringObstacleAvoidance.WhiskerConfiguration.SingleWithShortWhiskers:
                l = Quaternion.AngleAxis(whiskerAngle, Vector3.up) * transform.forward;
                r = Quaternion.AngleAxis(-whiskerAngle, Vector3.up) * transform.forward;
                if (Physics.Raycast(pos, transform.forward, out hitInfo, primaryDistance, obstacleLayers))
                    Hits.Add(GetRepulsionPoint(hitInfo));
                if (Physics.Raycast(pos, l, out hitInfo, secondaryRatio * primaryDistance, obstacleLayers))
                    Hits.Add(GetRepulsionPoint(hitInfo));
                if (Physics.Raycast(pos, r, out hitInfo, secondaryRatio * primaryDistance, obstacleLayers))
                    Hits.Add(GetRepulsionPoint(hitInfo));
                break;
            case SteeringObstacleAvoidance.WhiskerConfiguration.WhiskersOnly:
                l = Quaternion.AngleAxis(whiskerAngle, Vector3.up) * transform.forward;
                r = Quaternion.AngleAxis(-whiskerAngle, Vector3.up) * transform.forward;
                if (Physics.Raycast(pos, l, out hitInfo, primaryDistance, obstacleLayers))
                    Hits.Add(GetRepulsionPoint(hitInfo));
                if (Physics.Raycast(pos, r, out hitInfo, primaryDistance, obstacleLayers))
                    Hits.Add(GetRepulsionPoint(hitInfo));
                break;
        }

        if (Hits.Count > 0)
        {
            SeekTarget = Vector3.zero;
            foreach (Vector3 hit in Hits) SeekTarget += hit;
            SeekTarget /= Hits.Count;
        }

        SteeringSeek.DelegatedSteer(myMotor, SeekTarget);

    }

    private Vector3 GetRepulsionPoint(RaycastHit hitInfo)
    {
        return hitInfo.point + hitInfo.normal * repulsionDistance;
    }

}
