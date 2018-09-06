using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrmaeRateCounter : MonoBehaviour
{

    public float measurementWindow = 1.0f;

    private float framerateTimer = 0.0f;
    private float framerate = 0.0f;
    private int frameCount = 0;

    private System.DateTime lastFrameUpdate;

    // Use this for initialization
    void Start()
    {
        lastFrameUpdate = System.DateTime.Now;
        ResetTimer();
    }

    void ResetTimer()
    {
        framerateTimer = 0.0f;
        frameCount = 0;
    }

    // Update is called once per frame
    void Update()
    {

        float elapsedTime = (float)(System.DateTime.Now - lastFrameUpdate).TotalSeconds;

        lastFrameUpdate = System.DateTime.Now;

        frameCount++;
        framerateTimer += elapsedTime;
        if (framerateTimer >= measurementWindow)
        {
            frameCount = frameCount / (int)framerateTimer;
            ResetTimer();
        }
    }

    private void OnGUI()
    {
        if (framerate> 0f)
        {
            GUILayout.Label(frameCount.ToString("0.00"));
        }
    }
}
