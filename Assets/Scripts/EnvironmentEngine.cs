using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helpers;
using UnityEngine;
using Random = System.Random;

public class EnvironmentEngine : MonoBehaviour
{

    public float speed;
    public bool gameStarted;
    public bool gameFinished;

    private GameObject FloorEngine;
    private GameObject CameraGO;
    private Camera Camera;
    private GameObject FrontBackgroundEngine;
    private GameObject BackgroundProvider;
    private GameObject BackBackgroundEngine;

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
        SetSpeed();
        InitializeBackgroundPieceOrder();
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
        }

        GenerateEnvironment();

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
        FloorEngine.transform.Translate(Vector3.left * speed * Time.deltaTime);
        FrontBackgroundEngine.transform.Translate(Vector3.left*(speed/1.2f)*Time.deltaTime);
        BackBackgroundEngine.transform.Translate(Vector3.left*(speed/2f)*Time.deltaTime);

    }

    void GenerateEnvironment()
    {
        GenerateFloor();
        GenerateFrontBackground();
        GenerateBackBackground();

        CleanEnvironment();
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

        if((FirstBackBackground.transform.position.x + FirstBackBackground.GetComponent<SpriteRenderer>().bounds.size.x)< ScreenLimits.x)
        {
            Destroy(FirstBackBackground);
        }
    }
}
