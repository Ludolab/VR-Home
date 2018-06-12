using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleFlicker : MonoBehaviour {

    public float flickerProbability;
    public float minIntensity;
    public float maxIntensity;
    float transitionTime = 0.35f;
    public Light flameLight;

	// Use this for initialization
	void Start () {
        flameLight.intensity = minIntensity;
	}
	
	// Update is called once per fram
 
    void Update()
    {
        float liklihood = Random.Range(1, 100);
        if (liklihood < flickerProbability){
            StartCoroutine(LightShift(Random.Range(minIntensity, maxIntensity)));
        }
    }

    private IEnumerator LightShift(float newValue)
    {
        float oldValue = flameLight.intensity;
        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            flameLight.intensity = Mathf.Lerp(oldValue, newValue, t / transitionTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
