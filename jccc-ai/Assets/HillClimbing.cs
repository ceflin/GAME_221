using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class HillClimbing : MonoBehaviour {

    public float minParameterValue = 1.0f;
    public float maxParameterValue = 5.0f;
    public float startParameterValue = 3.0f;
    public float interval = 0.1f;
    public float maxSeconds = 30.0f;

    private float currentParameterValue;

    private SteeringObstacleAvoidance myObstacleAvoidance;
    private CharacterSteeringMotor motor;
    private int targetLayer = 0;

    private Vector3 startingPosition;
    private Quaternion startingOrientation;

    private float startTime;
    private bool hasStarted = false;

    private Dictionary<float, float> results = new Dictionary<float, float>();

    private Queue<float> parameterQueue = new Queue<float>();

    private float climberPosition;

    private System.Text.StringBuilder outputBuilder = new System.Text.StringBuilder();

	// Use this for initialization
	void Start () {
        myObstacleAvoidance = GetComponent<SteeringObstacleAvoidance>();
        motor = GetComponent<CharacterSteeringMotor>();
        targetLayer = LayerMask.NameToLayer("Target");

        startingPosition = transform.position;
        startingOrientation = transform.rotation;

        for (float queueValue = minParameterValue; queueValue <= maxParameterValue; queueValue += interval)
        {
            parameterQueue.Enqueue(queueValue);
        }

        //parameterQueue.Enqueue(startParameterValue);
        //parameterQueue.Enqueue(startParameterValue - interval);
        //parameterQueue.Enqueue(startParameterValue + interval);
        
        climberPosition = startParameterValue;
    }
	
	// Update is called once per frame
	void Update () {
	    if (!hasStarted)
        {
            if (parameterQueue.Count > 0)
            {
                motor.velocity = Vector3.zero;
                motor.rotation = 0;
                transform.position = startingPosition;
                transform.rotation = startingOrientation;
                currentParameterValue = parameterQueue.Dequeue();
                myObstacleAvoidance.primaryDistance = currentParameterValue;
                hasStarted = true;
                startTime = Time.time;
            }
            else
            {
                print("Finished with best value of " + climberPosition);
                this.enabled = false;
                print(outputBuilder.ToString());
            }
        }
        else
        {
            if (Time.time - startTime >= maxSeconds)
            {
                print("Parameter value " + currentParameterValue + " timed out");
                outputBuilder.AppendLine(string.Format("{0}\t{1}", currentParameterValue, maxSeconds));
                results.Add(currentParameterValue, maxSeconds);
                hasStarted = false;
            }
        }
	}

    void OnGUI()
    {
        GUILayout.Label(string.Format("{0} seconds", (int)(Time.time - startTime)));
    }

    void OnTriggerEnter(Collider other)
    {
        if (this.isActiveAndEnabled && other.gameObject.layer == targetLayer)
        {
            // Reset
            hasStarted = false;
            results.Add(currentParameterValue, Time.time - startTime);
            print("Parameter value " + currentParameterValue + ", result is " + results[currentParameterValue]);
            outputBuilder.AppendLine(string.Format("{0}\t{1}", currentParameterValue, results[currentParameterValue]));


            float left = climberPosition - interval, right = climberPosition + interval;

            if (false && results.ContainsKey(left) && results.ContainsKey(right))
            {
                // Check difference
                double deltaLeft = results[left] - results[climberPosition];
                double deltaRight = results[right] - results[climberPosition];

                // Currently we're looking for an energy; how much time we needed to get there.
                if (deltaLeft < 0 || deltaRight < 0)
                {
                    // Only enqueue further if we have an improvement; take the lesser of the two
                    if (deltaLeft < deltaRight && left > minParameterValue)
                    {
                        print("Improvement at " + left);
                        parameterQueue.Enqueue(left - interval);
                        climberPosition = left;
                    }
                    else
                    {
                        print("Improvement at " + right);
                        parameterQueue.Enqueue(right + interval);
                        climberPosition = right;
                    }
                }
            }



        }
    }
}
