using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedCollider : MonoBehaviour {
    
    public GameObject myStarter;
    public Dirt myDirt;

	// Use this for initialization
	void Start () {
        myStarter = null;
	}

	private void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.GetComponent<Starter>() != null && myStarter == null){
            myStarter = other.gameObject;
        }
        if (myDirt.digState == 1){
            StartCoroutine(myDirt.TakePlant());
        }
	}

	private void OnTriggerExit(Collider other)
	{
        if (other.gameObject.GetComponent<Starter>() != null)
        {
            myStarter = null;
        }
        myDirt.digState = 1;
	}
}
