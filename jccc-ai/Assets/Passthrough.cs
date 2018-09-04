using UnityEngine;
using System.Collections;

public class Passthrough : MonoBehaviour {

    public Vector3 positionAdjust;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void OnTriggerEnter(Collider other)
    {
        // Something hits us, move it
        other.transform.position += positionAdjust;
    }
}
