using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateLevel : MonoBehaviour
{
    Vector3 floorPosition;
    Vector3 playerPosition;

    public GameObject player;
    public GameObject floor;
    public InputField lvlWidth;
    public InputField lvlLength;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        floor.transform.localScale.Set(float.Parse(lvlLength.text), float.Parse(lvlWidth.text), 0);

    }

    public void CreateNewLevel()
    {
        floorPosition = new Vector3(0, 0, 0);
        playerPosition = floorPosition + new Vector3(0, 1.5f, 0);

        //floor.transform.localScale.Set(float.Parse(lvlLength.text), float.Parse(lvlWidth.text), 0); 

        GameObject.Instantiate<GameObject>(floor, floorPosition, Quaternion.Euler(90, 0, 0));
        GameObject.Instantiate<GameObject>(player, playerPosition, Quaternion.identity);
    }
}
