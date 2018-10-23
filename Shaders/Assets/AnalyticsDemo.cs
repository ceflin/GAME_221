using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsDemo : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Dictionary<string, object> additionalData = new Dictionary<string, object>();
        additionalData.Add("StartingNumber", 15);

        AnalyticsEvent.GameStart();
        AnalyticsEvent.Custom("custom_demo_event", additionalData);

        AnalyticsEvent.Custom("object_found_level1_1");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
