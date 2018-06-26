using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSkyFull : MonoBehaviour {

    public float percentThroughDay;
    public Material dayToSunset;
    public Material sunsetToNight;
    public Material nightToSunrise;
    public Material sunriseToDay;
    public Color daylightColor = new Color(1, 0.9568f, 0.8392f, 1);
    public Color sunsetColor = new Color(1, 0.9682f, 0.6462f, 1);
    public Color nightColor = new Color(0.946f, 0.929f, 1, 1);
    public Color sunriseColor = new Color(1, 0.9682f, 0.6462f, 1);
    public Light sunLight;
    //public Light bounceLight;
    public int secondsPerCycle;
    private float cycleStartTime = 0;

    private Vector3 sunDefaultPositionVector = new Vector3(-10, 0, -135);

    private static int nightTime = 20;
    private static int dayTime = 30;
    private static int transitionTime = (100 - (nightTime + dayTime)) / 4;
    private static Quaternion sunStartAngle = Quaternion.Euler(0, 190, 0);
    private static Vector3 axisOfSunRotation = new Vector3(5, 1, 2);

    // Use this for initialization
    void Start()
    {
        cycleStartTime = -1 * (secondsPerCycle / 4);
        percentThroughDay = 25; // starts with daylight
        applyChanges();
    }

    // Update is called once per frame
    void Update()
    {
        /* MANUAL TIME SHIFT: FOR DEBUGGING 
        if (Input.GetKey("right"))
        {
            percentThroughDay += 1f;
            if (percentThroughDay > 100)
            {
                percentThroughDay = 100;
            }
            //Debug.Log("percentThroughDay is " + percentThroughDay);
        }
        if (Input.GetKey("left"))
        {
            percentThroughDay -= 1f;
            if (percentThroughDay < 0)
            {
                percentThroughDay = 0;
            }
            //Debug.Log("percentThroughDay is " + percentThroughDay);
        } */
        if (Time.time - cycleStartTime >= secondsPerCycle) {
            cycleStartTime = Time.time;
        }
        percentThroughDay = Mathf.Lerp(0, 100, (Time.time - cycleStartTime) / secondsPerCycle);
        applyChanges();
    }

    public void applyChanges()
    {
        sunLight.transform.rotation = sunStartAngle * Quaternion.AngleAxis(Mathf.Lerp(0, 360, percentThroughDay/100.0f), axisOfSunRotation);
        if (percentThroughDay <= transitionTime)
        // Night to Sunrise
        {
            float percentThroughPhase = percentThroughDay / transitionTime;
            RenderSettings.skybox = nightToSunrise;
            nightToSunrise.SetFloat("_Blend", percentThroughPhase);
            sunLight.intensity = Mathf.Lerp(0, 1, percentThroughPhase);
            sunLight.color = sunriseColor;
        }
        else if (percentThroughDay > transitionTime && percentThroughDay <= (2 * transitionTime))
        // Sunrise to Day
        {
            float percentThroughPhase = (percentThroughDay - transitionTime) / transitionTime;
            RenderSettings.skybox = sunriseToDay;
            sunriseToDay.SetFloat("_Blend", percentThroughPhase);
            sunLight.intensity = Mathf.Lerp(1, 1.5f, percentThroughPhase);
            Color lightColor = Color.Lerp(sunriseColor, daylightColor, percentThroughPhase);
            sunLight.color = lightColor;
        }
        else if (percentThroughDay > (2 * transitionTime) && percentThroughDay <= (2 * transitionTime) + dayTime)
        // Day
        {
            float percentThroughPhase = (percentThroughDay - (2 * transitionTime)) / dayTime;
            RenderSettings.skybox = sunriseToDay;
            sunriseToDay.SetFloat("_Blend", 1);
            sunLight.intensity = 1.5f;
            sunLight.color = daylightColor;
        }
        else if (percentThroughDay > ((2 * transitionTime) + dayTime) && percentThroughDay <= (3 * transitionTime) + dayTime)
        // Day to Sunset
        {
            float percentThroughPhase = (percentThroughDay - (2 * transitionTime) - dayTime) / transitionTime;
            RenderSettings.skybox = dayToSunset;
            dayToSunset.SetFloat("_Blend", percentThroughPhase);
            sunLight.intensity = Mathf.Lerp(1.5f, 1, percentThroughPhase);
            Color lightColor = Color.Lerp(daylightColor, sunsetColor, percentThroughPhase);
            sunLight.color = lightColor;

        }
        else if (percentThroughDay > ((3 * transitionTime) + dayTime) && percentThroughDay <= (4 * transitionTime) + dayTime)
        // Sunset to Night
        {
            float percentThroughPhase = (percentThroughDay - (3 * transitionTime) - dayTime) / transitionTime;
            RenderSettings.skybox = sunsetToNight;
            sunsetToNight.SetFloat("_Blend", percentThroughPhase);
            sunLight.intensity = Mathf.Lerp(1, 0, percentThroughPhase);
            sunLight.color = sunsetColor;
        }
        else
        // Night
        {
            float percentThroughPhase = (percentThroughDay - (4 * transitionTime) - dayTime) / nightTime;
            RenderSettings.skybox = sunsetToNight;
            sunsetToNight.SetFloat("_Blend", 1);
            sunLight.intensity = 0;
            sunLight.color = sunsetColor;
        }
    }
}
