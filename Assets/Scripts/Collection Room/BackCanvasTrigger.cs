using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackCanvasTrigger : MonoBehaviour
{

    public FrameManager myManager;

    void OnTriggerEnter(Collider other)
    {
        myManager.setImage(other.gameObject);
    }

    void OnTriggerStay (Collider other)
    {
        myManager.mayHaveFoundController(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        myManager.mayHaveLostController(other.gameObject);
    }
}
