using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text lblLevelName;
    private LevelController gameController1;



    public Text txtLevelWidth;
    public Text txtLevelLength;
    public GameController gameController;


    // Use this for initialization
    void Start()
    {
        gameController1 = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();



        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        lblLevelName.text = gameController1.levelName;


        gameController.levelWidth = float.Parse(txtLevelWidth.text);
    }
}
