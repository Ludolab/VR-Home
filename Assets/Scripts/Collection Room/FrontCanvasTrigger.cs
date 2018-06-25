using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCanvasTrigger : MonoBehaviour {

    public FrameManager myManager;
    bool touchingCloth;

    void OnTriggerEnter(Collider other)
    {
        if (touchingCloth)
        {
            myManager.frontCollisionWithCloth(other);
        }
        if (other.gameObject == myManager.myCanvas)
        {
            touchingCloth = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == myManager.myCanvas)
        {
            touchingCloth = false;
        }
    }
}
