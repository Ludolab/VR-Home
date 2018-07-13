﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt : MonoBehaviour {

    public GameObject dirtParticles;
    Material myMaterial;
    SkinnedMeshRenderer skinnedMeshRenderer;
    int digState; // 0 = flat, 1 = hole, 2 = mound
    float wetness;
    public float waterTime;
    public float waterIncrement;
    public float digTime;
    bool inTransition;

	// Use this for initialization
	void Start () {
        myMaterial = GetComponent<Renderer>().material;
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        wetness = 0;
        digState = 0;
        inTransition = false;
	}
	
    // Update is called once per frame
    void Update()
    {
        /* MANUAL DIGGING: FOR DEBUGGING */
        if (Input.GetKey("right") && digState == 0)
        {
            StartCoroutine(DigHole());
        }
        if (Input.GetKey("left") && digState == 1)
        {
            StartCoroutine(CoverHole());
        }
        if (Input.GetKey("left") && digState == 2)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(0, 0);
            skinnedMeshRenderer.SetBlendShapeWeight(1, 0);
            digState = 0;
        }
    }

    public IEnumerator IncrementWetness(){
        //Debug.Log("Wetness is " + wetness);
        if (wetness < 1){
            float oldValue = wetness;
            //Debug.Log("We're incrementing wetness!");
            for (float t = 0; t < waterTime; t += Time.deltaTime)
            {
                wetness = Mathf.Lerp(oldValue, oldValue + waterIncrement, t / waterTime);
                myMaterial.SetFloat("_Wetness", wetness);
                yield return new WaitForEndOfFrame();
            }
            wetness = oldValue + waterIncrement;
            myMaterial.SetFloat("_Wetness", wetness);
        }
    }

    public IEnumerator DigHole (){
        if (digState == 0 && !inTransition)
        {
            inTransition = true;
            dirtParticles.GetComponent<ParticleSystem>().Play();
            for (float t = 0; t < digTime; t += Time.deltaTime)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(0, Mathf.Lerp(0, 100, t / digTime));
                yield return new WaitForEndOfFrame();
            }
            skinnedMeshRenderer.SetBlendShapeWeight(0, 100);
            digState = 1;
            dirtParticles.GetComponent<ParticleSystem>().Stop();
            inTransition = false;
        }
    }

    public IEnumerator CoverHole()
    {
        if (digState == 1 && !inTransition){
            inTransition = true;
            dirtParticles.GetComponent<ParticleSystem>().Play();
            for (float t = 0; t < digTime; t += Time.deltaTime)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(0, Mathf.Lerp(100, 0, t / digTime));
                skinnedMeshRenderer.SetBlendShapeWeight(1, Mathf.Lerp(0, 100, t / digTime));
                yield return new WaitForEndOfFrame();
            }
            skinnedMeshRenderer.SetBlendShapeWeight(0, 0);
            skinnedMeshRenderer.SetBlendShapeWeight(1, 100);
            digState = 2;
            dirtParticles.GetComponent<ParticleSystem>().Stop();
            inTransition = false;
        }
    }

}
