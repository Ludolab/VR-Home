﻿using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestFruit : MonoBehaviour {

    private bool noBeetles;
    private Plot plotIn;
    private InteractionBehaviour ib;
    private Rigidbody rb;
    private Collider col;

    private void Start() {
        rb = this.gameObject.GetComponent<Rigidbody>();

        col = this.gameObject.GetComponent<Collider>();
        rb.useGravity = false;
        col.isTrigger = true;
    }

    /*public void setNoBeetles(bool nb, InteractionManager m) {
        Debug.Log("Setting if plot plant is in has beetles. Value is: " + nb);
        noBeetles = nb;
        if (noBeetles) {
            ib.enabled = true;
            ib.manager = m;
        } else {
            ib.enabled = false;
        }
    }*/

    public void setPlot(Plot plot) {
        plotIn = plot;
    }

	private void OnTriggerExit(Collider other)
	{
        if(plotIn != null
           && other == plotIn.myDirt.SurfaceCollider.GetComponent<Collider>()
           && other.gameObject.transform.position.y <= gameObject.transform.position.y) {
            plotIn.SquishBeetles();
            rb.useGravity = true;
            col.isTrigger = false;
            plotIn.RemovePlant();
            TimeManager.instance.AddGarbage(gameObject);
        }
	}
}
