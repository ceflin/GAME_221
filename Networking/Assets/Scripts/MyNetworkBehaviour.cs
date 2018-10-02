using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkBehaviour : NetworkBehaviour {

    private NetworkManager myNetworkManager;

	// Use this for initialization
	void Start () {

        print("MyNetworkBehaviour.Start");
        myNetworkManager = GameObject.FindGameObjectWithTag("NetworkController").GetComponent<NetworkManager>();

    }

    public override void OnStartClient()
    {
        print("MyNetworkBehaviour.OnStartClient");
        //// Now how do I get my client?
        //if (GetComponent<NetworkIdentity>().isLocalPlayer)
        //{
        //    myNetworkManager.client.RegisterHandler(100, OnHello);
        //}
    }

    public void OnHello(NetworkMessage netMsg)
    {
        print("Something called hello here in MyNetworkBehaviour");
    }

}
