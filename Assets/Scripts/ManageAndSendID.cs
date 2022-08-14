using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[Serializable]
public class First_Result
{
    public Json_File result;
}


[Serializable]
public class Json_File
{
    public string name;
    public ReviewsInfo[] reviews;
}

[Serializable]
public class ReviewsInfo
{
    public string author_name;
    public string author_url;
    public string profile_photo_url;
    public int rating;
    public string relative_time_description;
    public string text;
    public int time;
}

namespace Geospatial_Basic{

public class ManageAndSendID : MonoBehaviour
{

    private string place_id;
    private string region = "jp";
    private string language = "ja";
    private string key = "";
    private string url = "https://maps.googleapis.com/maps/api/place/details/json?";

    public GameObject VPS_Demo;


    // Start is called before the first frame update
    public void SendIDAndCreateAvatar()
    {
      Debug.Log("click");
      url += "place_id=" + place_id;
      url += "&region=" + region;
      url += "&language=" + language;
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
            // 結果をテキストで表示
            string inputString = www.downloadHandler.text;
            Debug.Log(inputString);
            //JsonUtiliyを使用して、Jsonデータを処理
            First_Result first_Result = JsonUtility.FromJson<First_Result>(inputString);

            VPS_Demo = GameObject.Find("VPS_Demo");

            //reviesの数分のPrefabを生成する
            for(int i = 0; i < first_Result.result.reviews.Length - 1 ; i++)

            {
                string R_photo_url = first_Result.result.reviews[i].profile_photo_url;
                string Rname = first_Result.result.name;
                string Rauthor_name = first_Result.result.reviews[i].author_name;
                string Rtime_description = first_Result.result.reviews[i].relative_time_description;
                string Rtext = first_Result.result.reviews[i].text;
                int Rrating = first_Result.result.reviews[i].rating;

                VPS_Demo.GetComponent<SetObjectRealWorld>().CreateReviewerAvatar(R_photo_url, Rname, Rauthor_name, Rtime_description, Rtext, Rrating);
            }


        }
    }

    public void ChangePlaceID (string id)
    {
      place_id = id;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

}
