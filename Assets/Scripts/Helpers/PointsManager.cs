using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class PointsManager : MonoBehaviour {

    public List<RunPeetyPoint> points { get; set; }
    private const string URL = "http://api.maderaplayground.de/public/RunPeetyPoints";
    public string Error { get; set; }

    public void GetPoints()
    {
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create("URL");
        request.Method = "GET";
        HttpWebResponse response = (HttpWebResponse) request.GetResponse();
        string hallo = "";
    }

   
}
