using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class LevelDataLoadSave : MonoBehaviour
{
    private LevelData levelData = new LevelData();

    public GameObject sphereTemplate;

    // Use this for initialization
    void Start()
    {
        string[] fileNames = System.IO.Directory.GetFiles(".\\customlevels","*.lvl");

        StringBuilder sb = new StringBuilder();
        foreach (string fileName in fileNames)
        {
            sb.AppendLine(fileName);
        }
        print(sb.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LoadLevel()
    {
        LevelData level = LevelData.LoadFromFile("default.lvl");

        foreach (GameObject existingSphere in GameObject.FindGameObjectsWithTag("MovingSphere"))
        {
            Destroy(existingSphere);
        }

        foreach (MovingSphereData movingSphere in levelData.movingSpheres)
        {
            GameObject levelSphere = GameObject.Instantiate(sphereTemplate);
            levelSphere.transform.position = movingSphere.pos;
            levelSphere.GetComponent<MoveWithLateralClamp>().moveSpeed = movingSphere.moveSpeed;
        }
    }

    private void SaveLevel()
    {
        LevelData level = new LevelData();
        foreach (GameObject movingSphereGameObject in GameObject.FindGameObjectsWithTag("MovingSphere"))
        {
            MovingSphereData currentMovingSphere = new MovingSphereData();
            currentMovingSphere.pos = movingSphereGameObject.transform.position;
            currentMovingSphere.moveSpeed = movingSphereGameObject.GetComponent<MoveWithLateralClamp>().moveSpeed;

            levelData.movingSpheres.Add(currentMovingSphere);
        }
        level.SaveToFile("default.lvl");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Load Level"))
        {
            LoadLevel();
        }

        if (GUILayout.Button("Save Level"))
        {
            SaveLevel();
        }
    }
}

[Serializable]
public class LevelData
{
    public List<MovingSphereData> movingSpheres = new List<MovingSphereData>();

    public void SaveToFile(string fileName)
    {
        System.IO.File.WriteAllText(fileName, JsonUtility.ToJson(this, true));
        UnityEngine.MonoBehaviour.print(System.IO.Directory.GetCurrentDirectory());
    }

    public static LevelData LoadFromFile(string fileName)
    {
        return JsonUtility.FromJson<LevelData>(System.IO.File.ReadAllText(fileName));
    }
}

[Serializable]
public class MovingSphereData
{
    public Vector3 pos;
    public float moveSpeed;
}