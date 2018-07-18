using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class Dirt : MonoBehaviour {

    public Plot myPlot;
    public GameObject dirtParticles;
    public GameObject SurfaceCollider;
    Material myMaterial;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public bool starterPresent = false;
    public int digState; // 0 = flat, 1 = hole, 2 = planted, 3 = mound, 4 = flat on later day (no digging)
    float wetness;
    public float waterTime;
    public float waterIncrement;
    public float digTime;
    public float plantTime;
    bool inTransition;

    public bool noWeeds; // Won't let people dig if any weeds are in the dirt.

	// Use this for initialization
	void Start () {
        myMaterial = GetComponent<Renderer>().material;
        SurfaceCollider.layer = 0;
        wetness = 0;
        digState = 0;
        inTransition = false;
	}
	
    // Update is called once per frame
    void Update()
    {
        /* MANUAL DIGGING: FOR DEBUGGING 
        if (Input.GetKey("right") && digState == 0)
        {
            StartCoroutine(DigHole());
        }
        if (Input.GetKey("left") && digState == 1)
        {
            StartCoroutine(CoverHole());
        }
        if (Input.GetKey("left") && digState >= 2)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(0, 0);
            skinnedMeshRenderer.SetBlendShapeWeight(1, 0);
            digState = 0;
        } */
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
        //Debug.Log("digState is " + digState);
        //Debug.Log("inTransition is " + inTransition);
        if (digState == 0 && !inTransition && noWeeds)
        {
            inTransition = true;
            //dirtParticles.GetComponent<ParticleSystem>().Play();
            for (float t = 0; t < digTime; t += Time.deltaTime)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(0, Mathf.Lerp(0, 100, t / digTime));
                yield return new WaitForEndOfFrame();
            }
            skinnedMeshRenderer.SetBlendShapeWeight(0, 100);
            digState = 1;
            SurfaceCollider.layer = 8;
            //dirtParticles.GetComponent<ParticleSystem>().Stop();
            inTransition = false;
        }
    }

    public IEnumerator CoverHole()
    {
        if (digState == 2 && !inTransition){
            inTransition = true;
            //dirtParticles.GetComponent<ParticleSystem>().Play();
            for (float t = 0; t < digTime; t += Time.deltaTime)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(0, Mathf.Lerp(100, 0, t / digTime));
                skinnedMeshRenderer.SetBlendShapeWeight(1, Mathf.Lerp(0, 100, t / digTime));
                yield return new WaitForEndOfFrame();
            }
            skinnedMeshRenderer.SetBlendShapeWeight(0, 0);
            skinnedMeshRenderer.SetBlendShapeWeight(1, 100);
            digState = 3;
            SurfaceCollider.layer = 0;
            //dirtParticles.GetComponent<ParticleSystem>().Stop();
            inTransition = false;
            myPlot.AbsorbPlant();
        }
    }

    public void makeFlat(bool digable){
        skinnedMeshRenderer.SetBlendShapeWeight(0, 0);
        skinnedMeshRenderer.SetBlendShapeWeight(1, 0);
        if (digable)
        {
            digState = 0;
        }
        else
        {
            digState = 4;
        }
    }

    public IEnumerator TakePlant(){
        for (float t = 0; t < plantTime; t += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
        }
        if (starterPresent){
            digState = 2;
        }
    }

    public float getWetness() {
        return wetness;
    }

    public void setWetness(float w) {
        wetness = w;
        myMaterial.SetFloat("_Wetness", wetness);
    }
}
