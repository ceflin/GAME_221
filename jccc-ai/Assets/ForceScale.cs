using UnityEngine;
using System.Collections;

public class ForceScale : MonoBehaviour {

    private Vector3 originalScale;
    private Vector3 originalParentScale;

	// Use this for initialization
	void Start () {

        originalScale = transform.localScale;
        originalParentScale = transform.parent.localScale;
	
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 safe = originalScale;
        if (transform.parent.localScale.x == 0)
            safe.x = 0;
        else
            safe.x /= transform.parent.localScale.x;

        if (transform.parent.localScale.y == 0)
            safe.y = 0;
        else
            safe.y /= transform.parent.localScale.y;

        if (transform.parent.localScale.z == 0)
            safe.z = 0;
        else
            safe.z /= transform.parent.localScale.z;

        transform.localScale = safe;
	}
}
