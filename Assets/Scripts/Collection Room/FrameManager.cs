using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FrameManager : MonoBehaviour {

    private static List<ClothSphereColliderPair> canvasSphereColliders;
    private static List<ClothSphereColliderPair> canvasCapsuleColliders;

    public GameObject myCanvas;
    GameObject heldMedia;
    Material myMaterial;
    Texture defaultTexture;

    private const int NUM_CONTROLLERS = 2;
    private SteamVR_TrackedObject[] controllers = new SteamVR_TrackedObject[NUM_CONTROLLERS];

	// Use this for initialization
	void Start () {
        myMaterial = myCanvas.GetComponent<Renderer>().material;
        defaultTexture = myMaterial.mainTexture;
        SteamVR_ControllerManager manager = GameObject.Find("[CameraRig]").GetComponent<SteamVR_ControllerManager>();
        controllers[0] = manager.left.GetComponent<SteamVR_TrackedObject>();
        controllers[1] = manager.right.GetComponent<SteamVR_TrackedObject>();
        canvasSphereColliders = new List<ClothSphereColliderPair>();
        canvasCapsuleColliders = new List<ClothSphereColliderPair>();
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
                        canvasSphereColliders.Add(new ClothSphereColliderPair(controller.GetComponent<SphereCollider>()));
                        myCanvas.GetComponent<Cloth>().sphereColliders = canvasSphereColliders.ToArray();
                    }
                    if (input.GetHairTriggerUp())
                    {
                        canvasSphereColliders.Remove(new ClothSphereColliderPair(controller.GetComponent<SphereCollider>()));
                        myCanvas.GetComponent<Cloth>().sphereColliders = canvasSphereColliders.ToArray();
                    }
                }
            }
            catch (System.IndexOutOfRangeException)
            {
                //can't talk to controller, don't do anything
            }
        }
	}

    public void backCollisionWithCloth(Collider other) {
        for (int controllerIndex = 0; controllerIndex < NUM_CONTROLLERS; controllerIndex++)
        {
            try
            {
                if (controllers[controllerIndex] != null && other.gameObject == controllers[controllerIndex].gameObject && PickUpStretch.grabbableObjects[(int)controllerIndex] != null)
                {
                    SteamVR_TrackedObject controller = controllers[controllerIndex];
                    canvasSphereColliders.Remove(new ClothSphereColliderPair(controller.GetComponent<SphereCollider>()));
                    myCanvas.GetComponent<Cloth>().sphereColliders = canvasSphereColliders.ToArray();
                    heldMedia = PickUpStretch.grabbableObjects[(int)controllerIndex];
                    myMaterial.mainTexture = heldMedia.GetComponent<Renderer>().material.mainTexture;
                    heldMedia.SetActive(false);
                }
            }
            catch (System.IndexOutOfRangeException)
            {
                //can't talk to controller, don't do anything
            }
        }
    }

    public void frontCollisionWithCloth(Collider other)
    {
        for (int controllerIndex = 0; controllerIndex < NUM_CONTROLLERS; controllerIndex++)
        {
            try
            {
                if (controllers[controllerIndex] != null && other.gameObject == controllers[controllerIndex].gameObject)
                {
                    SteamVR_TrackedObject controller = controllers[controllerIndex];
                    canvasSphereColliders.Remove(new ClothSphereColliderPair(controller.GetComponent<SphereCollider>()));
                    myCanvas.GetComponent<Cloth>().sphereColliders = canvasSphereColliders.ToArray();
                    PickUpStretch.grabbableObjects[(int)controllerIndex] = heldMedia;
                    heldMedia.SetActive(true);
                    myMaterial.mainTexture = defaultTexture;
                }
            }
            catch (System.IndexOutOfRangeException)
            {
                //can't talk to controller, don't do anything
            }
        }
    }
}
