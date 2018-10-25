using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineToGlowObject : MonoBehaviour
{
    public Vector3 playerPos;
    public List<GameObject> glowObjects;
    public Dictionary<GameObject, Vector3> glowObsPos;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        
    }

    private void OnDrawGizmos()
    {
    }
}
