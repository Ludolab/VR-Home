﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class FrameManager : MonoBehaviour {

    private static List<ClothSphereColliderPair> canvasSphereColliders;
    private static List<ClothSphereColliderPair> canvasCapsuleColliders;
    private VideoPlayer vp;

    public GameObject myCanvas;
    GameObject heldMedia;
    Material myMaterial;

    private float transitionTime = 0.5f;
    private const int NUM_CONTROLLERS = 2;
    private SteamVR_TrackedObject[] controllers = new SteamVR_TrackedObject[NUM_CONTROLLERS];
    private bool[] controllersBehindCanvas = new bool[NUM_CONTROLLERS];

	// Use this for initialization
	void Start () {
        myMaterial = myCanvas.GetComponent<Renderer>().material;
        vp = myCanvas.GetComponent<VideoPlayer>();
        SteamVR_ControllerManager manager = GameObject.Find("[CameraRig]").GetComponent<SteamVR_ControllerManager>();
        controllers[0] = manager.left.GetComponent<SteamVR_TrackedObject>();
        controllers[1] = manager.right.GetComponent<SteamVR_TrackedObject>();
        controllersBehindCanvas[0] = false;
        controllersBehindCanvas[1] = false;
        canvasSphereColliders = new List<ClothSphereColliderPair>();
        canvasCapsuleColliders = new List<ClothSphereColliderPair>();
        canvasSphereColliders.Add(new ClothSphereColliderPair(controllers[0].GetComponent<SphereCollider>()));
        canvasSphereColliders.Add(new ClothSphereColliderPair(controllers[1].GetComponent<SphereCollider>()));
        myCanvas.GetComponent<Cloth>().sphereColliders = canvasSphereColliders.ToArray();
	}
	
	// Update is called once per frame
	void Update () {
        for (int controllerIndex = 0; controllerIndex < NUM_CONTROLLERS; controllerIndex++)
        {
            try {
                SteamVR_TrackedObject controller = controllers[controllerIndex];
                if (controller.gameObject.activeSelf){
                    SteamVR_Controller.Device input = SteamVR_Controller.Input((int)controller.index);
                    if (input.GetHairTriggerDown())
                    {
                        /*canvasSphereColliders.Add(new ClothSphereColliderPair(controller.GetComponent<SphereCollider>()));
                        myCanvas.GetComponent<Cloth>().sphereColliders = canvasSphereColliders.ToArray();*/
                        if (controllersBehindCanvas[controllerIndex]){
                            removeImage(controllerIndex);
                        }
                    }
                    if (input.GetHairTriggerUp())
                    {
                        /*canvasSphereColliders.Remove(new ClothSphereColliderPair(controller.GetComponent<SphereCollider>()));
                        myCanvas.GetComponent<Cloth>().sphereColliders = canvasSphereColliders.ToArray();*/
                    }
                }
            }
            catch (System.IndexOutOfRangeException)
            {
                //can't talk to controller, don't do anything
            }
        }
	}

    public void setImage(GameObject obj) {
        Transform t = obj.transform.Find("Quad");
        if (t != null)
        {
            GameObject image = t.gameObject;
            VideoPlayer vid = image.GetComponent<VideoPlayer>();
            if (vid != null)
            {
                vp.clip = vid.clip;
                vp.Play();
            }
            else
            {
                //use image texture
                Debug.Log("Transitioning to Display");
                TransitionToDisplay(image.GetComponent<Renderer>().material.mainTexture);
            }
            heldMedia = obj;
            PickUpStretch pickerUpper = heldMedia.GetComponent<PickUpStretch>();
            pickerUpper.Release(pickerUpper.holder);
            heldMedia.SetActive(false);
        } else {
            Debug.Log("Could not find Quad");
        }
    }

    public void mayHaveFoundController(GameObject other)
    {
        for (int controllerIndex = 0; controllerIndex < NUM_CONTROLLERS; controllerIndex++)
        {
            if (controllers[controllerIndex] != null && other == controllers[controllerIndex].gameObject)
            {
                controllersBehindCanvas[controllerIndex] = true; 
            }
       }
   }

    public void mayHaveLostController(GameObject other)
    {
        for (int controllerIndex = 0; controllerIndex < NUM_CONTROLLERS; controllerIndex++)
        {
            if (controllers[controllerIndex] != null && other == controllers[controllerIndex].gameObject)
            {
                controllersBehindCanvas[controllerIndex] = false;
            }
         }
    } 

    public void removeImage(int controllerIndex)
    {
        Debug.Log("removeImage called");
        TransitionToDefault();
        vp.Stop();
        vp.clip = null;
        heldMedia.SetActive(true);
        heldMedia.GetComponent<PickUpStretch>().Grab(controllerIndex);
        heldMedia = null;
    }

    private IEnumerator TransitionToDefault()
    {
        float oldValue = myMaterial.GetFloat("Threshold");
        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            myMaterial.SetFloat("Threshold", Mathf.Lerp(oldValue, 0, t / transitionTime));
            yield return new WaitForEndOfFrame();
        }
        myMaterial.SetFloat("Threshold", 0);
    }

    private IEnumerator TransitionToDisplay(Texture newTex)
    {
        myMaterial.SetTexture("Display (RGB)", newTex);
        float oldValue = myMaterial.GetFloat("Threshold");
        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            myMaterial.SetFloat("Threshold", Mathf.Lerp(oldValue, 1, t / transitionTime));
            yield return new WaitForEndOfFrame();
        }
        myMaterial.SetFloat("Threshold", 1);
    }
}
