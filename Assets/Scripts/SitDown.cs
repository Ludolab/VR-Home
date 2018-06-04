using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitDown : MonoBehaviour {

    private const float SIT_TIME = 1.0f; //time required in sitting position before overlay disappears, in seconds
    private const float SIT_HEIGHT = 1.0f; //maximum y-position above floor that counts as sitting, in meters
    private const float FADE_IN_TIME = 2.0f; //time to fade everything in after sitting

    private Transform camTransform;
    private bool sittingInProgress = false;
    private Renderer[] childRenderers;

    private void Awake()
    {
        camTransform = Camera.main.transform;

        //Deactivate everything else except gameObject and [CameraRig], add it to a list to reactivate

        //Activate dark walls
        int childCount = gameObject.transform.childCount;
        childRenderers = new Renderer[childCount];
        for (int i = 0; i < childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;
            child.SetActive(true);
            childRenderers[i] = child.GetComponent<Renderer>();
        }
        /*childRenderers = gameObject.transform.GetComponentsInChildren<Renderer>();
        foreach(Renderer rend in childRenderers)
        {
            rend.gameObject.SetActive(true);
        }*/
    }
    
    private void Update ()
    {
        if (!sittingInProgress && IsSitting())
        {
            sittingInProgress = true;
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
                sittingInProgress = false;
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
        Complete();
    }

    private void Complete()
    {
        print("Sitting complete");
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        //Reactivate everything, make it transparent
        //Fade everything in
        //Fade dark walls out
        for (float t = 0; t < FADE_IN_TIME; t += Time.deltaTime)
        {
            SetEverythingTransparency(t / FADE_IN_TIME);
            yield return new WaitForEndOfFrame();
        }
        SetEverythingTransparency(1);
        Destroy(gameObject);
    }

    private void SetEverythingTransparency(float transparency)
    {
        //set everything (with a renderer)'s color alpha to transparency

        float inverseTransparency = 1 - transparency;
        foreach (Renderer rend in childRenderers)
        {
            Color oldCol = rend.material.color;
            rend.material.color = new Color(oldCol.r, oldCol.g, oldCol.b, inverseTransparency);
        }
    }
}
