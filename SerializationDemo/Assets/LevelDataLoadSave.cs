using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class LevelDataLoadSave : MonoBehaviour
{
    private LevelData levelData = new LevelData();

    public GameObject sphereTemplate;
    public GameObject obstacleTemplate;

    private LevelController gameController;

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

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LoadLevel(string levelName)
    {
        LevelData level = LevelData.LoadFromFile(levelName + ".lvl");

        gameController.levelName = level.GetLevelName();

        //Ground Plane
        float groundPlaneX = level.groundPlane.width;
        float groundPlaneY = level.groundPlane.length;
        Transform groundPlane = GameObject.FindGameObjectWithTag("GroundPlane").transform;
        groundPlane.localScale = new Vector3(groundPlaneX, groundPlaneY, 1);

        //Player FOV
        gameController.playerFOV = level.fieldOfView;

        gameController.playerDetectDist = level.playerDetectDist;

        //Obstacles
        foreach (GameObject existingObstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Destroy(existingObstacle);
        }
        foreach (ObstacleData obstacle in level.obstacles)
        {
            GameObject levelObstacle = GameObject.Instantiate(obstacleTemplate);
            levelObstacle.transform.position = obstacle.position;
            levelObstacle.transform.localEulerAngles = new Vector3(1, obstacle.rotation, 1);
        }




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

    private void SaveLevel(string levelName)
    {
        LevelData level = new LevelData();

        level.SetLevelName(gameController.levelName);

        //Ground Plane
        Transform groundPlane = GameObject.FindGameObjectWithTag("GroundPlane").transform;
        level.groundPlane.width = groundPlane.localScale.x;
        level.groundPlane.length = groundPlane.localScale.y;

        //Player FOV
        level.fieldOfView = gameController.playerFOV;

        level.playerDetectDist = gameController.playerDetectDist;

        //Obstacles
        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            ObstacleData newObstacle = new ObstacleData();
            newObstacle.position = obstacle.transform.position;
            newObstacle.rotation = obstacle.transform.localEulerAngles.y;

            level.obstacles.Add(newObstacle);
        }
        //level.obstacleData.position = GameObject.FindGameObjectWithTag("Obstacle").transform.position;
        //level.obstacleData.rotation = GameObject.FindGameObjectWithTag("Obstacle").transform.localEulerAngles.y;



        foreach (GameObject movingSphereGameObject in GameObject.FindGameObjectsWithTag("MovingSphere"))
        {
            MovingSphereData currentMovingSphere = new MovingSphereData();
            currentMovingSphere.pos = movingSphereGameObject.transform.position;
            currentMovingSphere.moveSpeed = movingSphereGameObject.GetComponent<MoveWithLateralClamp>().moveSpeed;

            levelData.movingSpheres.Add(currentMovingSphere);
        }
        level.SaveToFile(levelName + ".lvl");
    }

    private string loadLevelName = "default";
    private string saveLevelName = "default";

    private void OnGUI()
    {
        if (GUILayout.Button("Load Level"))
        {
            LoadLevel(loadLevelName);
        }
        GUILayout.Label("Level name to load");
        loadLevelName = GUILayout.TextField(loadLevelName);

        if (GUILayout.Button("Save Level"))
        {
            SaveLevel(saveLevelName);
        }
        GUILayout.Label("Level name to save");
        saveLevelName = GUILayout.TextField(saveLevelName);
    }
}

[Serializable]
public class GroundPlane
{
    public float width;
    public float length;
}

[Serializable]
public class ObstacleData
{
    public Vector3 position;
    public float rotation;
}

[Serializable]
public class LevelData
{
    public List<MovingSphereData> movingSpheres = new List<MovingSphereData>();

    public string levelName;
    //public float width;
    //public float height;
    //public Vector2 groundPlaneSize;
    public GroundPlane groundPlane = new GroundPlane();

    public float fieldOfView;

    public float playerDetectDist;

    //public ObstacleData obstacleData = new ObstacleData();
    public List<ObstacleData> obstacles = new List<ObstacleData>();


    public string GetLevelName()
    {
        return levelName;
    }

    public void SetLevelName(string levelName)
    {
        this.levelName = levelName;
    }

    public float PlayerFieldOfView
    {
        get
        {
            return fieldOfView;
        }
        set
        {
            fieldOfView = value;
        }
    }

    public float DetectDist
    {
        get
        {
            return playerDetectDist;
        }
        set
        {
            playerDetectDist = value;
        }
    }



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