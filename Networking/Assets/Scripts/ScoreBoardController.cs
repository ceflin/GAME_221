using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.UI;

public class ScoreBoardController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GameObject.FindGameObjectWithTag("NetworkController").GetComponent<CustomNetworkControl>().RegisterScoreboard(this);

    }

    // Update is called once per frame
    void Update()
    {
        StringBuilder scoreBuilder = new StringBuilder();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            scoreBuilder.AppendFormat("{0}: {1}\n", player.GetComponent<PlayerScore>().playerName, player.GetComponent<PlayerScore>().score);
        }

        GetComponent<Text>().text = scoreBuilder.ToString();
    }
}
