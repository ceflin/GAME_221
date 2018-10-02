using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DisablePlayerControllsOnRemote : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        if (!GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            GetComponent<PlayerController>().enabled = false;
        }

    }

}
