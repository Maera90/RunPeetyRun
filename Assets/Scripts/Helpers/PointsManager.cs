using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Scripts.Helpers
{
    public class PointsManager : MonoBehaviour {

        public RunPeetyPointSet pointsSet { get; set; }
        private const string URL = "https://api.maderaplayground.de/RunPeetyPoints";
        public string Error { get; set; }

        public Text LabelPrefab;
        public InputField NameField;
        public EnvironmentEngine EnviromentEngine;
        public GameObject PostPointsGO;
        public GameObject PointContainer;
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

                   
                    int counter = 1;
                    foreach (RunPeetyPoint singlePoint in pointsSet.data)
                    {
                        Text instantiate = Instantiate(LabelPrefab, PointContainer.transform);
                        instantiate.text = singlePoint.name + " - " + singlePoint.points;
                        Color color;
                        switch (counter)
                        {
                            case 1:
                                color = Color.yellow;
                                break;
                            case 2:
                                color = Color.gray;
                                break;
                            case 3:
                                color = Color.red;
                                break;
                            default:
                                color = Color.white;
                                break;
                        }
                        instantiate.color = color;
                        counter++;
                    }

                }
            }
        }

        public void SavePoints()
        {
            string name = NameField.text;
            RunPeetyPoint point = new RunPeetyPoint();
            if (!string.IsNullOrEmpty(name))
            {
                point.name = name;
                point.points = EnviromentEngine.points;

                StartCoroutine(PostPoints(point));
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
                else
                {
                    PostPointsGO.SetActive(false);
                }
               
            }

        }

        
   
    }
}
