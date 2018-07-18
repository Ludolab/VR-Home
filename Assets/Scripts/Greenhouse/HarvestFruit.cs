﻿using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestFruit : MonoBehaviour {

    private bool noBeetles;
    private Plot plotIn;
    private Rigidbody rb;
    private InteractionBehaviour ib;
    private Collider col;

    private void Start() {
        rb = this.gameObject.GetComponent<Rigidbody>();
        ib = this.gameObject.GetComponent<InteractionBehaviour>();
        col = this.gameObject.GetComponent<Collider>();
        ib.enabled = true;
        rb.useGravity = false;
        col.isTrigger = true;
    }

    public void setNoBeetles(bool nb) {
        noBeetles = nb;
        if (noBeetles) {
            ib.enabled = true;
        } else {
            ib.enabled = false;
        }
    }

    public void setPlot(Plot plot) {
        plotIn = plot;
    }

	private void OnTriggerExit(Collider other)
	{
        if(other == plotIn.myDirt.SurfaceCollider.GetComponent<Collider>()) {
            rb.useGravity = true;
            col.isTrigger = false;
        }
	}
}
