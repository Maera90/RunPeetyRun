using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{

    public Peety _peety;
    private GameObject EnviromentEngineGO;

    private EnvironmentEngine _environmentEngine;
	// Use this for initialization
	void Start () {

        EnviromentEngineGO = GameObject.Find("EnvironmentEngine");
	    _environmentEngine = EnviromentEngineGO.GetComponent<EnvironmentEngine>();
        
        
	}
	
	// Update is called once per frame
	void Update () {

	    #region Inputs

	    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
	    {
	       


	        if (_environmentEngine.gameFinished)
	        {
	           ReloadGame();
	        }

	        if (!_environmentEngine.gameStarted)
	        {
	            _peety.Run();
	            _environmentEngine.StartGame();

	        }
	        else
	        {
	            _peety.Jump();
	        }       
	    }

        //Start Game!!
	    if (Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
	    {
	        if (!_environmentEngine.gameStarted && !_environmentEngine.gameFinished)
	        {
                _peety.Run();
                _environmentEngine.StartGame();
	        }
	    }

        //Ducking
	    if (Input.GetKeyDown(KeyCode.DownArrow))
	    {
	        _peety.Duck();
	    }

	    #endregion

        //Set Peety Speed Running animation speed
	   


	}

    public void ReloadGame()
    {
        Scene sceneToReload = SceneManager.GetActiveScene();
        SceneManager.LoadScene(sceneToReload.name);
    }

    
}
