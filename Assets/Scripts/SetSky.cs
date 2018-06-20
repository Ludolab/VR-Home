using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SetSky : MonoBehaviour {

    public float percentThroughDay;
    public Material dayToSunset;
    public Material sunsetToNight;
    public Color daylightColor = Color.white; //new Color(1, 0.9568f, 0.8392f, 1);
    public Color sunsetColor = Color.red;//new Color(1, 0.9682f, 0.6462f, 1);
    public Color nightColor = Color.blue;//new Color(0.946f, 0.929f, 1, 1);
    public Light sun;
    public Light bounceLight;

	// Use this for initialization
    void Start () {
        percentThroughDay = 0f; // starts with daylight
        applyChanges();
	}
	
	// Update is called once per frame
	void Update () {
        /* MANUAL TIME SHIFT: FOR DEBUGGING */
        if (Input.GetKeyDown("right"))
        {
            percentThroughDay += 1f;
            if (percentThroughDay > 100)
            {
                percentThroughDay = 100;
            }
            //Debug.Log("percentThroughDay is " + percentThroughDay);
        }
        if (Input.GetKeyDown("left"))
        {
            percentThroughDay -= 1f;
            if (percentThroughDay < 0)
            {
                percentThroughDay = 0;
            }
            //Debug.Log("percentThroughDay is " + percentThroughDay);
        }
        applyChanges();
	}

    public void applyChanges() {
        float angleLateral = (percentThroughDay / 2) + 220;
        if (percentThroughDay > 40)
        // After Sunset    
        {
            float percentThroughEvening = (percentThroughDay - 40) / 60;
            RenderSettings.skybox = sunsetToNight;
            sunsetToNight.SetFloat("_Blend", percentThroughEvening);
            float angleVertical = Mathf.Lerp(20, -5, percentThroughEvening);
            sun.transform.rotation = Quaternion.Euler(angleVertical, angleLateral, angleVertical);
            sun.intensity = Mathf.Lerp(1.5f, 0, percentThroughEvening);
            sun.color = sunsetColor;
            bounceLight.color = Color.Lerp(sunsetColor, nightColor, percentThroughEvening);
            bounceLight.intensity = Mathf.Lerp(sun.intensity/2, 0.0f, percentThroughEvening);
        }
        else
        // Before Sunset    
        {
            float percentThroughMorning = percentThroughDay / 40;    
            RenderSettings.skybox = dayToSunset;
            dayToSunset.SetFloat("_Blend", percentThroughMorning);
            float angleVertical = Mathf.Lerp(60, 20, percentThroughMorning);
            sun.transform.rotation = Quaternion.Euler(angleVertical, angleLateral, angleVertical);
            sun.intensity = 1.5f;
            Color lightColor = Color.Lerp(daylightColor, sunsetColor, percentThroughMorning);
            sun.color = lightColor;
            bounceLight.color = lightColor;
            bounceLight.intensity = sun.intensity/2;
        }
    }

}
