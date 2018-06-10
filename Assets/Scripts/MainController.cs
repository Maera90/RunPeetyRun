using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    private GameObject PeetyGO;
    private Peety _peety;
    private GameObject EnviromentEngineGO;

    private EnvironmentEngine _environmentEngine;
	// Use this for initialization
	void Start () {
		PeetyGO = GameObject.Find("Peety");
	    _peety = PeetyGO.GetComponent<Peety>();
        EnviromentEngineGO = GameObject.Find("EnvironmentEngine");
	    _environmentEngine = EnviromentEngineGO.GetComponent<EnvironmentEngine>();
	}
	
	// Update is called once per frame
	void Update () {

	    #region Inputs

	    if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
	    {
	        if (!_environmentEngine.gameStarted)
	        {
	            _peety.Run();
	            _environmentEngine.gameStarted = true;

	        }
	        else
	        {
	            _peety.Jump();
	        }       
	    }

	    #endregion

    }
}
