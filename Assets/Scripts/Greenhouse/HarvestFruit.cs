﻿using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestFruit : MonoBehaviour {

    private bool noBeetles;
    private Plot plotIn;
    private Rigidbody rb;
    private Collider col;

    private void Awake() {
        rb = this.gameObject.GetComponent<Rigidbody>();
        col = this.gameObject.GetComponent<Collider>();
        rb.useGravity = false;
        rb.isKinematic = true;
        col.isTrigger = true;
    }

    public void SetPlot(Plot plot) {
        plotIn = plot;
    }

	private void OnTriggerExit(Collider other)
	{
        if(plotIn != null
           && other == plotIn.myDirt.SurfaceCollider.GetComponent<Collider>()
           && other.gameObject.transform.position.y <= gameObject.transform.position.y) {
            Debug.Log("Plant now exiting plot");
            plotIn.SquishBeetles();
            rb.useGravity = true;
            rb.isKinematic = false;
            col.isTrigger = false;
            plotIn.RemovePlant();
            TimeManager.instance.AddGarbage(gameObject);
        }
	}
}
