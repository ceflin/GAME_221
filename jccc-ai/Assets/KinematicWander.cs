using UnityEngine;
using System.Collections;

public class KinematicWander : MonoBehaviour {

    public float turnChangeRate = 0.1f;     // Can change as much as 0.1 of its turn rate per second

    private float currentTurnRate = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        float turnChangeAmount = UnityEngine.Random.Range(-1.0f, 1.0f) * turnChangeRate;
        currentTurnRate += turnChangeAmount;



        GetComponent<CharacterCommands>().Turn(currentTurnRate);

        GetComponent<CharacterCommands>().Move(transform.forward, 1);

	}
}
