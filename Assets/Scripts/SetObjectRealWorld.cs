using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using Google.XR.ARCoreExtensions;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Geospatial_Basic{

public class SetObjectRealWorld : MonoBehaviour
{
    public AREarthManager EarthManager;
    public VpsInitializer Initializer;
    public SetClosePlaceTag SetTag;
    public Text OutputText;

    public double HeadingThreshold = 25;
    public double HorizontalThreshold = 20;

    public double Latitude;
    public double Longitude;
    public double Altitude;
    public double Heading;

    public GameObject ContentPrefab;
    public GameObject NameTagPrefab;
    public GameObject ReviewerPrefab;
    public GameObject targetObject;

    GameObject displayObject;
    public ARAnchorManager AnchorManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void ClickAndSendPresentLocation()
    {
      GeospatialPose pose = EarthManager.CameraGeospatialPose;
      string CurrentLat = pose.Latitude.ToString();
      string CurrentLng = pose.Longitude.ToString();

      SetTag.StartGetClosePlace(CurrentLat,CurrentLng);
    }

    public void ActivateSetCloseTag(string name, string lat, string lng, string id)
    {

      GeospatialPose pose = EarthManager.CameraGeospatialPose;
      Altitude = pose.Altitude + 5.0f;

      Latitude = Convert.ToDouble(lat);
      Longitude = Convert.ToDouble(lng);
      Debug.Log(lat);

      Quaternion quaternion = Quaternion.AngleAxis(180f - (float)Heading, Vector3.up);
      ARGeospatialAnchor anchor = AnchorManager.AddAnchor(Latitude,Longitude,Altitude,quaternion);


      GameObject tagObject = (GameObject)Instantiate(NameTagPrefab, anchor.transform);
      tagObject.transform.LookAt(targetObject.transform);
      tagObject.transform.Find("Name").GetComponent<Text>().text = name;
      tagObject.GetComponent<ManageAndSendID>().ChangePlaceID(id);
    }

    public void CreateReviewerAvatar(string profile_photo_url, string Rname, string Rauthor_name, string Rtime_description, string Rtext, int Rrating)
    {
      StartCoroutine(GetTexture(profile_photo_url, Rname, Rauthor_name, Rtime_description, Rtext, Rrating));

    }

    IEnumerator GetTexture(string profile_photo_url, string Rname, string Rauthor_name, string Rtime_description, string Rtext, int Rrating)
    {
      GeospatialPose pose = EarthManager.CameraGeospatialPose;
      Altitude = pose.Altitude -1.5f;
      System.Random random = new System.Random();
      Latitude = random.NextDouble() * 0.00002 + pose.Latitude - 0.00001;
      Longitude = random.NextDouble() * 0.00002 + pose.Longitude - 0.00001;

      Quaternion quaternion = Quaternion.AngleAxis(180f - (float)Heading, Vector3.up);
      ARGeospatialAnchor anchor = AnchorManager.AddAnchor(Latitude,Longitude,Altitude,quaternion);

      WWW www2 = new WWW(profile_photo_url);
      yield return www2;

      GameObject reviewerObject = (GameObject)Instantiate(ReviewerPrefab, anchor.transform);
      GameObject obj = reviewerObject.transform.Find("Review_Panel").gameObject;


      

      obj.transform.Find("ReviewerPic").gameObject.GetComponent<RawImage>().texture = www2.texture;
      obj.transform.Find("Place").gameObject.GetComponent<Text>().text = Rname;
      obj.transform.Find("ReviewerName").gameObject.GetComponent<Text>().text = Rauthor_name;
      obj.transform.Find("Time").gameObject.GetComponent<Text>().text = Rtime_description;
      obj.transform.Find("Review").gameObject.GetComponent<Text>().text = Rtext;

      for(int j = 0; j < Rrating; j++)
      {
        obj.transform.Find("Rate").gameObject.GetComponent<Text>().text += "★" ;
      }
    }


    // Update is called once per frame
    void Update()
    {
      string status = "";

      if(!Initializer.IsReady || EarthManager.EarthTrackingState != TrackingState.Tracking)
      {
        return;
      }
      GeospatialPose pose = EarthManager.CameraGeospatialPose;

      if (pose.HeadingAccuracy > HeadingThreshold ||
                  pose.HorizontalAccuracy > HorizontalThreshold)
            {
                status = "低精度：周辺を見回してください";
            }
            else
            {
                status = "高精度：High Tracking Accuracy";
                if(displayObject == null)
                {
                  Altitude = pose.Altitude + 5.0f;
                  Quaternion quaternion = Quaternion.AngleAxis(180f - (float)Heading, Vector3.up);

                  ARGeospatialAnchor anchor = AnchorManager.AddAnchor(Latitude,Longitude,Altitude,quaternion);

                  if(anchor != null)
                  {
                    displayObject = Instantiate(ContentPrefab, anchor.transform);
                  }
                }

            }

      ShowTrackingInfo(status,pose);
    }

    void ShowTrackingInfo(string status, GeospatialPose pose)
    {
      OutputText.text = string.Format(
               "\n" +
               "Latitude/Longitude: {0}°, {1}°\n" +
               "Horizontal Accuracy: {2}m\n" +
               "Altitude: {3}m\n" +
               "Vertical Accuracy: {4}m\n" +
               "Heading: {5}°\n" +
               "Heading Accuracy: {6}°\n" +
               "{7} \n"
               ,
               pose.Latitude.ToString("F6"),  //{0}
               pose.Longitude.ToString("F6"), //{1}
               pose.HorizontalAccuracy.ToString("F6"), //{2}
               pose.Altitude.ToString("F2"),  //{3}
               pose.VerticalAccuracy.ToString("F2"),  //{4}
               pose.Heading.ToString("F1"),   //{5}
               pose.HeadingAccuracy.ToString("F1"),   //{6}
               status //{7}
           );
    }
}

}
