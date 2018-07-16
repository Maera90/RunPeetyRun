using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Helpers
{
    public class PointsManager : MonoBehaviour {

        public RunPeetyPointSet pointsSet { get; set; }
        private const string URL = "http://api.maderaplayground.de/public/RunPeetyPoints";
        public string Error { get; set; }

        void Start()
        {
            StartCoroutine(GetPoints());
        }

        IEnumerator GetPoints()
        {
            using (UnityWebRequest www = UnityWebRequest.Get(URL))
            {
                yield return www.SendWebRequest();
                if (www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    string jsonString = www.downloadHandler.text;
                    pointsSet = new RunPeetyPointSet();
                    pointsSet = JsonUtility.FromJson<RunPeetyPointSet>(jsonString);
                }
            }
        }

        
   
    }
}
