using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWhenHit : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        print("GONG");
        gameObject.GetComponent<AudioSource>().Play();
    }
}
