using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCount : MonoBehaviour
{

    public int numberOfValues = 1000;

    public float countBudgetMs = 10;

    private int nextCountValue = 0;

    private float runningTotal = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //float total = 0.0f;
        //for (int i = 0; i < numberOfValues; i++)
        //{
        //    total += UnityEngine.Random.Range(-1f, 1f);
        //}
        //print(total);

        System.DateTime endTime = System.DateTime.Now.AddMilliseconds(countBudgetMs);

        while (System.DateTime.Now < endTime && nextCountValue < numberOfValues)
        {
            nextCountValue++;
            runningTotal += UnityEngine.Random.Range(-1f, 1f);
        }

        if (nextCountValue >= numberOfValues)
        {
            print(runningTotal);
            runningTotal = 0.0f;
            nextCountValue = 0;
        }
    }
}
