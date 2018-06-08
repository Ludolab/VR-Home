using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRiser : MonoBehaviour {

    float _originalHeight;

    void Start () {
        _originalHeight = transform.position.y;
    }
    
   // void Update () {
  //      if (HeightManager.Instance) transform.position = new Vector3(transform.position.x, _originalHeight * HeightManager.Instance.playerHeight / HeightManager.Instance.standardHeight, transform.position.z);
   // }
}
