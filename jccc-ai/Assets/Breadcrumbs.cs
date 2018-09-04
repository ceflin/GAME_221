using UnityEngine;
using System.Collections;

public class Breadcrumbs : MonoBehaviour {

    public GameObject breadcrumb;

    public float timeStep = 0.25f;

    private float timeCount = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        timeCount += Time.deltaTime;

        if (timeCount > timeStep)
        {
            timeCount -= timeStep;
            GameObject newBreadcrumb = GameObject.Instantiate(breadcrumb);
            newBreadcrumb.transform.position = transform.position;
            newBreadcrumb.name = "breadcrumb";
        }

	}
}
