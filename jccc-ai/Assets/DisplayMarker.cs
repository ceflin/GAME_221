using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class DisplayMarker : MonoBehaviour {

	// Use this for initialization
	void Start () {

        int tryParse = -1;
        if (!int.TryParse(transform.parent.parent.name, out tryParse))
        {
            transform.parent.parent.name = GameObject.FindGameObjectsWithTag("WaypointFlag").Length.ToString();
        }
        RefreshDisplay();

    }
	
	// Update is called once per frame
	void Update () {

        string displayText = string.Empty;
        for (int i = 0; i < transform.childCount; i++)
        {
            displayText += transform.GetChild(i).name;
        }
        if (transform.parent.parent.name != displayText)
        {
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            Start();
        }

	}

    void RefreshDisplay()
    {
        GameObject numerals = GameObject.Find("Numerals");
        string label = transform.parent.parent.name;

        List<GameObject> sprites = new List<GameObject>();
        for (int i = 0; i < label.Length; i++)
        {
            string character = label.Substring(i, 1);
            GameObject originalSprite = numerals.transform.Find("numberStrip_" + character).gameObject;
            GameObject cloneSprite = GameObject.Instantiate(originalSprite);
            cloneSprite.transform.parent = this.transform;
            cloneSprite.transform.position = cloneSprite.transform.parent.position;
            cloneSprite.transform.LookAt(cloneSprite.transform.position + cloneSprite.transform.parent.forward);
            cloneSprite.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            cloneSprite.name = character;
            sprites.Add(cloneSprite);
        }

        if (sprites.Count == 2)
        {
            Vector3 adjust = new Vector3(0, 0, 0.235f);
            sprites[0].transform.position -= adjust;
            sprites[1].transform.position += adjust;
        }
        UnityEditor.SceneView.RepaintAll();
    }
}
