using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitDown : MonoBehaviour {

    private Transform camTransform;

    private void Awake()
    {
        camTransform = Camera.main.transform;
    }

	void Update () {
		if (camTransform.position.y <= 1)
        {
            print("OK!");
            gameObject.SetActive(false);
        }
	}
}
