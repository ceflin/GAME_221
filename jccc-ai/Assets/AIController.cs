using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour {

    public GameObject aiAgent;
    public GameObject player;

    public enum BehaviorModel
    {
        SeekPlayer,
        Wander
    }

    public BehaviorModel behaviorModel = BehaviorModel.SeekPlayer;

	// Use this for initialization
	void Start () {

        switch (behaviorModel)
        {
            case BehaviorModel.SeekPlayer:
                aiAgent.GetComponent<KinematicWander>().enabled = false;
                aiAgent.GetComponent<KinematicSeek>().enabled = true;
                break;
            case BehaviorModel.Wander:
                aiAgent.GetComponent<KinematicSeek>().enabled = false;
                aiAgent.GetComponent<KinematicWander>().enabled = true;
                break;
        }


	}
	
	// Update is called once per frame
	void Update () {

        switch (behaviorModel)
        {
            case BehaviorModel.SeekPlayer:
                aiAgent.GetComponent<KinematicSeek>().destination = player.transform.position;
                aiAgent.GetComponent<KinematicWander>().enabled = false;
                aiAgent.GetComponent<KinematicSeek>().enabled = true;
                break;
            case BehaviorModel.Wander:
                aiAgent.GetComponent<KinematicWander>().enabled = true;
                aiAgent.GetComponent<KinematicSeek>().enabled = false;
                break;
        }

	}
}
