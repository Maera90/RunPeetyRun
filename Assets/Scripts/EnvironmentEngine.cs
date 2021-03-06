﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helpers;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.UI;
using Random = System.Random;

public class EnvironmentEngine : MonoBehaviour
{

    public float speed;
    public bool gameStarted;
    public bool gameFinished;
    public int points = 100;
    public float speedMultiplicator = 1;
    public GameObject Rock;
    public GameObject Meteor;

    private GameObject FloorEngine;
    private GameObject CameraGO;
    private Camera Camera;
    private GameObject FrontBackgroundEngine;
    private GameObject BackgroundProvider;
    private GameObject BackBackgroundEngine;
    private GameObject ObstacleEngine;
    public GameObject PostPoints;
    public GameObject PointList;
    public GameObject RestartGameButton;
    
    private List<GameObject> obstacles = new List<GameObject>();
    #region texts
    public GameObject PointsText;
    public GameObject FinalPointsText;
    public GameObject GameOverText;
    public GameObject PushSpaceText;
    public GameObject RunPeetyText;
    #endregion


    #region PieceCounters

    private GameObject lastFrontBackgroundPiece;
    private GameObject lastFloorPiece;
    private List<GameObject> FloorPieces;
    private List<GameObject> BackgroundPieces;

    #endregion



    private

    // Use this for initialization
    void Start()
    {
        FloorEngine = GameObject.Find("FloorEngine");
        CameraGO = GameObject.Find("Main Camera");
        Camera = CameraGO.GetComponent<Camera>();
        FrontBackgroundEngine = GameObject.Find("FrontBackgroundEngine");
        BackgroundProvider = GameObject.Find("BackgroundProvider");
        BackBackgroundEngine = GameObject.Find("BackBackgroundEngine");
        ObstacleEngine = GameObject.Find("ObstacleEngine");
        SetSpeed();
        InitializeBackgroundPieceOrder();
        obstacles.Add(GameObject.FindGameObjectWithTag("Rock"));
        obstacles.Add(GameObject.FindGameObjectWithTag("Meteor"));
    }

    private void InitializeBackgroundPieceOrder()
    {
        List<GameObject> FrontBackgroundPieces = GameObject.FindGameObjectsWithTag("FrontBackground").Reverse().ToList();
        lastFrontBackgroundPiece = FrontBackgroundPieces.Last();

    }


    // Update is called once per frame

    void Update()
    {

        if (gameStarted && !gameFinished)
        {
            MoveBackgroound();
            GenerateEnvironment();
            UpdatePoints();
            UpdateSpeedMultiplicator();
        }

    }

    private void UpdateSpeedMultiplicator()
    {
        float multiplicator = points / 100f;
        multiplicator = multiplicator / 2f;
        multiplicator = multiplicator + 1f;
        speedMultiplicator = multiplicator;
    }


    private float nextTime = 0;
    private void UpdatePoints()
    {
        if (nextTime == 0)
        {
            nextTime = Time.time;
        }
        if (Time.time >= nextTime)
        {
            points = points + 1;
            Text textComponen = PointsText.GetComponent<Text>();
            textComponen.text = "SCORE: " + points;
            nextTime += 1;
            Debug.Log("SpeedMultiplicator: " + speedMultiplicator);
        }

    }


    void GameStart()
    {
        SetSpeed();
    }

    void SetSpeed()
    {
        speed = 10;
    }

    void MoveBackgroound()
    {
        FloorEngine.transform.Translate(Vector3.left * (speed * speedMultiplicator) * Time.deltaTime);
        ObstacleEngine.transform.Translate(Vector3.left * (speed * speedMultiplicator) * Time.deltaTime);
        FrontBackgroundEngine.transform.Translate(Vector3.left * ((speed * speedMultiplicator) / 1.2f) * Time.deltaTime);
        BackBackgroundEngine.transform.Translate(Vector3.left * ((speed * speedMultiplicator) / 2f) * Time.deltaTime);
        //Vector2 targetPosition = new Vector2(10, 0);
        Vector2 moveTo = new Vector2(-12f,-5.96f);
        Meteor.transform.position = Vector2.MoveTowards(Meteor.transform.position,moveTo,(speed * speedMultiplicator)*Time.deltaTime);
    }

    private float generateTime = 0;
    void GenerateEnvironment()
    {
      
        GenerateFloor();
        GenerateFrontBackground();
        GenerateBackBackground();
       
        CleanEnvironment();

        if (generateTime == 0)
        {
            generateTime = Time.time;
        }

        if (Time.time >= generateTime)
        {
            GenerateObstacles();
            generateTime +=2;
        }
        

    }

    void GenerateFloor()
    {
        FloorPieces = GameObject.FindGameObjectsWithTag("FloorPiece").ToList();

        if (FloorPieces.Count <= 2)
        {
            GameObject lastPiece = FloorPieces.Last();
            GameObject newFloorPiece = Instantiate(FloorPieces.FirstOrDefault());
            GameObject floorEngine = GameObject.Find("FloorEngine");
            newFloorPiece.gameObject.transform.parent = floorEngine.transform;

            float lastPiecewidth = lastPiece.GetComponent<SpriteRenderer>().bounds.size.x;
            newFloorPiece.transform.position = new Vector3(lastPiece.transform.position.x + (lastPiecewidth - 0.5f), lastPiece.transform.position.y);

        }
    }

    void GenerateBackBackground()
    {
        BackgroundPieces = GameObject.FindGameObjectsWithTag("BackBackgroundPiece").ToList();
        if (BackgroundPieces.Count <= 2)
        {
            GameObject lastPiece = BackgroundPieces.Last();
            GameObject newBackPiece = Instantiate(BackgroundPieces.FirstOrDefault());
            newBackPiece.gameObject.transform.parent = BackBackgroundEngine.transform;
            float lastPiecewidth = lastPiece.GetComponent<SpriteRenderer>().bounds.size.x;
            newBackPiece.transform.position = new Vector3(lastPiece.transform.position.x + (lastPiecewidth - 0.1f), lastPiece.transform.position.y);

        }
    }
    void GenerateFrontBackground()
    {
        List<GameObject> FrontBackgroundPieces = GameObject.FindGameObjectsWithTag("FrontBackground").ToList();

        if (FrontBackgroundPieces.Count <= 3)
        {
            GameObject randomFrontBackground = GetRandomBackground(BackgroundType.Front);
            GameObject newFrontBackground = Instantiate(randomFrontBackground);
            newFrontBackground.gameObject.transform.parent = FrontBackgroundEngine.transform;
            newFrontBackground.gameObject.tag = "FrontBackground";

            GameObject lastPiece = lastFrontBackgroundPiece;
            float lastPieceWidth = lastPiece.GetComponent<SpriteRenderer>().bounds.size.x;
            newFrontBackground.transform.position = new Vector3(lastPiece.transform.position.x + (lastPieceWidth - 0.1f), lastPiece.transform.position.y);
            lastFrontBackgroundPiece = newFrontBackground;
        }

    }

    void GenerateObstacles()
    {

        
        Random random = new Random();
        int randomIndex = random.Next(0,obstacles.Count);
        GameObject randomObstacle = obstacles[randomIndex];

        string tag = randomObstacle.tag;
        Vector2 targetPosition = new Vector2(10,10);
        switch (tag)
        {
            case "Rock":
                targetPosition = new Vector2(10,randomObstacle.gameObject.transform.position.y);
                randomObstacle.gameObject.transform.position = targetPosition;
                break;
            case "Meteor":
                targetPosition = new Vector2(10,0);
                Vector2 moveTo = new Vector2(-11f,-5f);
                randomObstacle.gameObject.transform.position = Vector2.MoveTowards(targetPosition,moveTo,10*Time.deltaTime);
                break;
        }

      
    }



    private GameObject GetRandomBackground(BackgroundType type)
    {
        GameObject backgroundPiece = new GameObject();
        System.Random random = new Random();

        if (type == BackgroundType.Front)
        {
            List<GameObject> FrontBackgrounds = GameObject.FindGameObjectsWithTag("FrontBackgroundSource").ToList();
            int index = random.Next(FrontBackgrounds.Count);
            backgroundPiece = FrontBackgrounds[index];

        }

        return backgroundPiece;
    }

    void CleanEnvironment()
    {
        Vector3 ScreenLimits = new Vector3(Camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x - 0.5f, 0);
        FloorPieces = GameObject.FindGameObjectsWithTag("FloorPiece").ToList();
        GameObject firstFloor = FloorPieces.FirstOrDefault();

        List<GameObject> FrontBackgroundPieces = GameObject.FindGameObjectsWithTag("FrontBackground").ToList();
        GameObject FirstFrontBackground = FrontBackgroundPieces.FirstOrDefault();

        GameObject FirstBackBackground = GameObject.FindGameObjectsWithTag("BackBackgroundPiece").ToList().FirstOrDefault();

        if ((firstFloor.transform.position.x + firstFloor.GetComponent<SpriteRenderer>().bounds.size.x) < ScreenLimits.x)
        {
            Destroy(firstFloor);
        }

        if ((FirstFrontBackground.transform.position.x + FirstFrontBackground.GetComponent<SpriteRenderer>().bounds.size.x) < ScreenLimits.x)
        {
            Destroy(FirstFrontBackground);
        }

        if ((FirstBackBackground.transform.position.x + FirstBackBackground.GetComponent<SpriteRenderer>().bounds.size.x) < ScreenLimits.x)
        {
            Destroy(FirstBackBackground);
        }

        List<GameObject> Obstacles = GameObject.FindGameObjectsWithTag("Rock").ToList();
        GameObject FirstObstacle = Obstacles.OrderBy(o => o.transform.position.x).First();
        if (FirstObstacle.transform.position.x < ScreenLimits.x)
        {
            RepositionObstable(FirstObstacle);
        }
    }

    public void FinishGame()
    {
        //PointsManager manager = new PointsManager();
        //manager.GetPoints();
        gameFinished = true;
        GameOverText.SetActive(true);

        Text totalpoints = FinalPointsText.GetComponent<Text>();
        totalpoints.text = points + " Punkte!";
        FinalPointsText.SetActive(true);

        PointsText.SetActive(false);
        RestartGameButton.SetActive(true);
        PostPoints.SetActive(true);
        PointList.SetActive(true);
    }

    public void StartGame()
    {
        gameStarted = true;
        PushSpaceText.SetActive(false);
        RunPeetyText.SetActive(false);
        PointsText.SetActive(true);
    }

    public void RepositionObstable(GameObject gameObject)
    {

    }
}
