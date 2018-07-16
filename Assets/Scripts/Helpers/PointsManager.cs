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
            RunPeetyPoint point = new RunPeetyPoint()
            {
                name = "Daniel",
                points = 666
            };
            StartCoroutine(PostPoints(point));
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

        IEnumerator PostPoints(RunPeetyPoint point)
        {

            string pointJson = JsonUtility.ToJson(point);
            Encoder encoder = new Encoder();

            string encodedPoint = encoder.encodeString(pointJson);
            Dictionary<string,string> postData = new Dictionary<string, string>();
            postData.Add("encodedPoint",encodedPoint);

            using (UnityWebRequest www = UnityWebRequest.Post(URL, postData))
            {
                yield return www.SendWebRequest();
                if (www.isHttpError)
                {
                    Debug.Log(www.error);
                }
               
            }

        }

        
   
    }
}
