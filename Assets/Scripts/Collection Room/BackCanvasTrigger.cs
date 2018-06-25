﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackCanvasTrigger : MonoBehaviour {

    public FrameManager myManager;
    bool touchingCloth;

    void OnTriggerEnter(Collider other){
        if (touchingCloth)
        {
            myManager.backCollisionWithCloth(other);
        }
        if (other.gameObject == myManager.myCanvas)
        {
            touchingCloth = true;
        }
    }

    void OnTriggerExit(Collider other){
        if (other.gameObject == myManager.myCanvas)
        {
            touchingCloth = false;
        }
    }
}
