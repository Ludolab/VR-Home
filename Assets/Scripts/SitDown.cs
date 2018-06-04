using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitDown : MonoBehaviour {

    private const float SIT_TIME = 1.0f; //time required in sitting position before overlay disappears, in seconds
    private const float SIT_HEIGHT = 1.0f; //maximum y-position above floor that counts as sitting, in meters
    private Transform camTransform;
    private bool sitting = false;

    private void Awake()
    {
        camTransform = Camera.main.transform;
    }
    
	private void Update ()
    {
		if (IsSitting())
        {
            StartCoroutine(WaitSitting());
        }
	}

    private bool IsSitting()
    {
        return camTransform.position.y <= 1;
    }

    private IEnumerator WaitSitting()
    {
        for (float t = 0; t < SIT_TIME; t += Time.deltaTime)
        {
            if (!IsSitting())
            {
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
        Complete();
    }

    private void Complete()
    {
        print("OK!");
        gameObject.SetActive(false);
    }
}
