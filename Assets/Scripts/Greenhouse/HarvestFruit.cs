using Leap.Unity.Interaction;
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
        ib.moveObjectWhenGrasped = false;
        rb.useGravity = false;
        col.isTrigger = true;
    }

    public void setNoBeetles(bool nb) {
        noBeetles = nb;
        if (noBeetles) {
            Debug.Log("Can now pick fruit");
            ib.moveObjectWhenGrasped = true;
        } else {
            ib.moveObjectWhenGrasped = false;
        }
    }

    public void setPlot(Plot plot) {
        plotIn = plot;
    }

	private void OnTriggerExit(Collider other)
	{
        if(other == plotIn.myDirt.SurfaceCollider.GetComponent<Collider>()) {
            Debug.Log("Leaving dirt.");
            rb.useGravity = true;
            col.isTrigger = false;
        }
	}
}
