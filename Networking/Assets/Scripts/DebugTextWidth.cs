using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugTextWidth : MonoBehaviour {

    private Text myText;

	// Use this for initialization
	void Start () {
        myText = GetComponent<Text>();
        myText.horizontalOverflow = HorizontalWrapMode.Overflow;
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            string startText = myText.text;

            myText.text = "Hi";
            print(myText.preferredWidth.ToString());
            myText.text = "Well hi yourself";
            print(myText.preferredWidth.ToString());
            // That actually works.
            // So, we can use this to calculate string width cumulatively and determine lines.
            // Handy.
            // Tech design in steps.


            myText.text = startText;
        }
	}
}
