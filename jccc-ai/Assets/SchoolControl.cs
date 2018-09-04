using UnityEngine;
using System.Collections;

public class SchoolControl : MonoBehaviour {

    public float neighborhoodRange = 3.0f;
    public float SqrNeighborhoodRange { get; private set; }
    public float arriveInnerRadius = 0.5f;
    public float arriveOuterRadius = 1.0f;
    public float separationDecay = 1.0f;

    [Range(0f, 1f)]
    public float cohesionWeight = 0.33f;
    [Range(0f, 1f)]
    public float separationWeight = 0.33f;
    [Range(0f, 1f)]
    public float velocityMatchWeight = 0.33f;

    public ISteerable[] members;

	// Use this for initialization
	void Start () {
        members = new ISteerable[transform.childCount];
        int i = 0;
        foreach (Transform child in transform) members[i++] = child.GetComponent<ISteerable>();
    }

    void Update()
    {
        SqrNeighborhoodRange = neighborhoodRange * neighborhoodRange;

        float totalWeights = cohesionWeight + separationWeight + velocityMatchWeight;
        cohesionWeight /= totalWeights;
        separationWeight /= totalWeights;
        velocityMatchWeight /= totalWeights;
    }

}
