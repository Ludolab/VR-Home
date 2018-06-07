using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SetSky : MonoBehaviour {

    private float percentThroughDay;
    public Material dayToSunset;
    public Material sunsetToNight;
    public GameObject sun;

	// Use this for initialization
    void Start () {
        percentThroughDay = 0f; // will be set based on actual time
        RenderSettings.skybox = dayToSunset;
        dayToSunset.SetFloat("_Blend", (percentThroughDay / 50));
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("right"))
        {
            percentThroughDay += 1f;
            if (percentThroughDay > 100)
            {
                percentThroughDay = 100;
            }
            Debug.Log("percentThroughDay is " + percentThroughDay);
            applyChanges();
        }
        if (Input.GetKeyDown("left"))
        {
            percentThroughDay -= 1f;
            if (percentThroughDay < 0)
            {
                percentThroughDay = 0;
            }
            Debug.Log("percentThroughDay is " + percentThroughDay);
            applyChanges();
        }
	}

    private void applyChanges() {
        if (percentThroughDay > 50)
        {
            RenderSettings.skybox = sunsetToNight;
            sunsetToNight.SetFloat("_Blend", (percentThroughDay - 50) / 50);
            float angleVertical = (((percentThroughDay - 50) / 50)*-30)+10;
            float angleLateral = (percentThroughDay/2) - 30;
            sun.transform.rotation = Quaternion.Euler(angleVertical, angleLateral, angleVertical);
        }
        else
        {
            RenderSettings.skybox = dayToSunset;
            dayToSunset.SetFloat("_Blend", (percentThroughDay / 50));
            float angleVertical = (((percentThroughDay) / 50) * -40) + 50;
            float angleLateral = (percentThroughDay/2) - 30;
            sun.transform.rotation = Quaternion.Euler(angleVertical, angleLateral, angleVertical);
        }
    }
}
