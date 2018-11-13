using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meep : MonoBehaviour
{
    public GameObject sphere;
    public GameObject cube;
    public int sphereWeight = 5;
    public int cubeWeight = 15;

    // Use this for initialization
    void Start()
    {
        int totalWeight = sphereWeight + cubeWeight;

        int roll = Random.Range(1, totalWeight + 1);

        if (roll <= sphereWeight)
        {
            GameObject temp = GameObject.Instantiate(sphere);
            temp.gameObject.transform.position = this.transform.position;
        }
        else
        {
            GameObject temp = GameObject.Instantiate(cube);
            temp.gameObject.transform.position = this.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
