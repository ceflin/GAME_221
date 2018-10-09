using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScore : NetworkBehaviour
{
    [SyncVar]
    public int score = 0;

    [SyncVar]
    public string playerName = string.Empty;

    public int Score
    {
        get { return score; }
        set
        {
            if (isServer)
                score = value;
        }
    }

    public int spacePressCount = 0;
    private ChatController myChat;

    // Use this for initialization
    void Start()
    {
        if (GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            print("Player score start");
            myChat = GameObject.FindGameObjectWithTag("ChatSystem").GetComponent<ChatController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            if (myChat.localPlayerName != this.playerName)
            {
                CmdSetName(myChat.localPlayerName);
            }
        }
    }

    [Command]
    public void CmdSetName(string playerName)
    {
        this.playerName = playerName;
    }
}
