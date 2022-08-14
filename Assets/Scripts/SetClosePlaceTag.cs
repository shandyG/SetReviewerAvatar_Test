using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[Serializable]
public class SpotResult{
    public GeoFile[] results;
}

[Serializable]
public class GeoFile
{
    public LocationInfo geometry;
    public string name;
    public string place_id;
}

[Serializable]
public class LocationInfo{
    public LatLngInfo location;
}

[Serializable]
public class LatLngInfo
{
    public string lat;
    public string lng;
}

namespace Geospatial_Basic{
public class SetClosePlaceTag : MonoBehaviour
{
  private string keyword = "";
  private string language = "ja";

  private int radius = 150;
  private string key = "";
  private string url = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?";

  public SetObjectRealWorld SetObjectRealWorld;

  public GameObject TagPrefab;
    // Start is called before the first frame update
    //public void ClickAndSetPlaceTag()
    public void StartGetClosePlace(string latC, string lngC)
    {
      string location = latC + "," + lngC;

      url += "keyword=" + keyword;
      url += "&language=" + language;
      url += "&location=" + location;
      url += "&radius=" + radius;
      url += "&key=" + key;


      StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string inputString = www.downloadHandler.text;
            Debug.Log(inputString);
            SpotResult spot_Result = JsonUtility.FromJson<SpotResult>(inputString);
            for(int i = 0; i < spot_Result.results.Length -1; i++)
            {

              string nameB = spot_Result.results[i].name;
              string latB = spot_Result.results[i].geometry.location.lat;
              string lngB = spot_Result.results[i].geometry.location.lng;
              string idB = spot_Result.results[i].place_id;

              SetObjectRealWorld.ActivateSetCloseTag(nameB, latB, lngB, idB);

              /*
              Debug.Log(spot_Result.results[0].name);
              Debug.Log(spot_Result.results[1].name);
              Debug.Log(spot_Result.results[1].geometry.location.lat);
              Debug.Log(spot_Result.results[2].geometry.location.lat);
              Debug.Log(spot_Result.results.Length　-1);
              */
            }

        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
}
