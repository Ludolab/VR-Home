using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleFlicker : MonoBehaviour {

    public float flickerProbability;
    public float minIntensity;
    public float maxIntensity;
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
            flameLight.intensity = Random.Range(minIntensity, maxIntensity);
        }
    }
}
