using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class CustomNetworkControl : NetworkManager
{

    #region Fields

    public string playerName;
    private ChatController myChat;

    #endregion

    #region Custom message types

    public class ChatMessage : MessageBase
    {
        public string sender;
        public string message;
    }

    #endregion

    #region MonoBehaviour events

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Network Manager events

    // Called on the client when connected to the server.
    public override void OnClientConnect(NetworkConnection conn)
    {
        print("Received connect message");
        myChat = GameObject.FindGameObjectWithTag("ChatSystem").GetComponent<ChatController>();
        client.Send(2001, new StringMessage(playerName));
    }

    #endregion

    #region Network control commands
    public void StartNetworkHost()
    {
        print("Starting host and registering handler, I think");
        StartHost();

        RegisterServerListeners();
        RegisterClientListeners();

    }

    public void StartNetworkClient()
    {
        print("Starting client and registering handler, I think");
        StartClient();

        RegisterClientListeners();
    }

    public void SendChatMessage(string message)
    {
        client.Send(3000, new StringMessage(message));
    }

    #endregion

    #region Client listeners

    private void RegisterClientListeners()
    {
        client.RegisterHandler(2002, OnNameAssigned);
        client.RegisterHandler(2003, OnOtherPlayerJoinedGame);
        client.RegisterHandler(3001, OnChatMessageReceived);
    }

    public void OnNameAssigned(NetworkMessage netMsg)
    {
        string playerName = netMsg.ReadMessage<StringMessage>().value;
        print("Client player name assigned to " + playerName);
        myChat.SetLocalPlayerName(playerName);
    }

    public void OnOtherPlayerJoinedGame(NetworkMessage netMsg)
    {
        string playerName = netMsg.ReadMessage<StringMessage>().value;
        print("Other player joined message recieved, name is " + playerName);
        myChat.AnnouncePlayer(playerName);
    }

    public void OnChatMessageReceived(NetworkMessage netMsg)
    {
        ChatMessage received = netMsg.ReadMessage<ChatMessage>();

        myChat.OnChatMessageReceived(received.sender, received.message);
    }

    #endregion

    #region Server listeners

    private void RegisterServerListeners()
    {
        NetworkServer.RegisterHandler(2001, OnPlayerNameReceived);
        NetworkServer.RegisterHandler(3000, OnPlayerSendChatMessage);
    }

    public void OnPlayerNameReceived(NetworkMessage netMsg)
    {
        string playerName = netMsg.ReadMessage<StringMessage>().value;
        print("Received player name " + playerName);
        playerName = myChat.SetPlayerName(playerName, netMsg.conn.connectionId);
        NetworkServer.SendToClient(netMsg.conn.connectionId, 2002, new StringMessage(playerName));
        NetworkServer.SendToAll(2003, new StringMessage(playerName));
    }

    public void OnPlayerSendChatMessage(NetworkMessage netMsg)
    {
        string message = netMsg.ReadMessage<StringMessage>().value.Trim();
        if (message.Length > 100)
        {
            message = message.Substring(0, 100).Trim();
        }
        string senderName = myChat.GetNameByConnectionID(netMsg.conn.connectionId);

        //string broadcastMessage = string.Format("{0}: {1}", senderName, message);
        //NetworkServer.SendToAll(3001, new StringMessage(broadcastMessage));
        ChatMessage chatMessage = new ChatMessage() { sender = senderName, message = message };
        NetworkServer.SendToAll(3001, chatMessage);
    }

    #endregion

}
