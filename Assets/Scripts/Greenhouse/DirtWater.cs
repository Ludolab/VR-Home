using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtWater : MonoBehaviour {

    public Dirt myDirt;

    void OnParticleCollision(GameObject other)
    {
        if (other.transform.parent.gameObject.GetComponent<WateringCan>() != null)
        {
            StartCoroutine(myDirt.IncrementWetness());
        }
    }
}
