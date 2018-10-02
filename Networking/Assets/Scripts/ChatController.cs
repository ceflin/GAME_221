using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ChatController : MonoBehaviour
{
    private Dictionary<int, string> namesByConnectionID = new Dictionary<int, string>();
    private Dictionary<string, int> connectionIDsByName = new Dictionary<string, int>();
    public int maxNameLength = 20;
    private string localPlayerName;

    public List<string> messages = new List<string>();

    public void OnChatMessageReceived(string sender, string message)
    {
        messages.Add(string.Format("[User chat] {0}: {1}", sender, message));

    }

    public string GetNameByConnectionID(int connectionID)
    {
        return namesByConnectionID[connectionID];
    }

    public void SetLocalPlayerName(string playerName)
    {
        localPlayerName = playerName;
    }

    public void AnnouncePlayer(string playerName)
    {
        print("Enter: " + playerName);
        messages.Add(string.Format("--- Player {0} has entered ---", playerName));

    }

    private void Awake()
    {
        print("Chat awakes...");
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private string EnsureUnique(string playerName, int connectionID)
    {
        //Base case: not actually changing the name.
        if (namesByConnectionID.ContainsKey(connectionID) && namesByConnectionID[connectionID] == playerName)
            return playerName;

        //Somebody else has this name; return altered form of it.
        int suffix = 0;
        while (connectionIDsByName.ContainsKey(playerName))
        {
            string suffixString = (++suffix).ToString();
            while (playerName.Length + suffixString.Length > maxNameLength)
                playerName = playerName.Substring(0, playerName.Length - 1);
        }

        return playerName;
    }

    internal string SetPlayerName(string playerName, int connectionID)
    {
        playerName = EnsureUnique(playerName, connectionID);

        if (namesByConnectionID.ContainsKey(connectionID))
        {
            //Remove from both indexes first
            connectionIDsByName.Remove(namesByConnectionID[connectionID]);
            namesByConnectionID.Remove(connectionID);
        }

        connectionIDsByName.Add(playerName, connectionID);
        namesByConnectionID.Add(connectionID, playerName);

        return playerName;
    }
}
