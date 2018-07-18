﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedCollider : MonoBehaviour {
    
    public Starter myStarter;
    public Dirt myDirt;

	// Use this for initialization
	void Start () {
        myStarter = null;
	}

	private void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.GetComponent<StarterCollider>() != null && myStarter == null){
            myStarter = other.gameObject.GetComponent<StarterCollider>().myStarter.GetComponent<Starter>();
            if (myDirt.digState == 1)
            {
                StartCoroutine(myDirt.TakePlant());
                Debug.Log(myStarter == null);
            }
        }
	}

	private void OnTriggerExit(Collider other)
	{
        if (other.gameObject.GetComponent<StarterCollider>() != null)
        {
            myStarter = null;
            myDirt.digState = 1;
        }
	}
}
