using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{

  private GameObject camera23;
  public GameObject plane23;
    // Start is called before the first frame update
    void Start()
    {
      camera23 = GameObject.Find("AR Camera");
    }

    // Update is called once per frame
    void Update()
    {
      plane23.transform.LookAt(camera23.transform);
      plane23.transform.Rotate(new Vector3(0f, -180f , 0f));
    }
}
