using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class txtPlayerName : MonoBehaviour {

    private CustomNetworkControl networkControl;

	// Use this for initialization
	void Start () {
        networkControl = GameObject.FindGameObjectWithTag("NetworkController").GetComponent<CustomNetworkControl>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AssignNameToNetworkControl()
    {
        networkControl.playerName = GetComponent<InputField>().text;
    }
}
