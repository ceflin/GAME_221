using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BindCameraToPlayer : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        if (GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            Transform mainCamera = Camera.main.transform;
            mainCamera.parent = transform.Find("EyeMount");
            mainCamera.position = mainCamera.parent.position;
            mainCamera.localScale = Vector3.one;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
