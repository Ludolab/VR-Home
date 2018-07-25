using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestFruit : MonoBehaviour
{

    private bool noBeetles;
    private Plot plotIn;
    private Rigidbody rb;
    private Collider col;
    private bool pulledUp = false;
    private bool removedFromPlot = false;

    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        col = this.gameObject.GetComponent<Collider>();
        rb.useGravity = false;
        rb.isKinematic = true;
        col.isTrigger = true;
    }

    public void SetPlot(Plot plot)
    {
        plotIn = plot;
    }

	private void OnTriggerExit(Collider other)
	{
        if(!pulledUp && plotIn != null
           && other == plotIn.myDirt.SurfaceCollider.GetComponent<Collider>()) {
            pulledUp = true;
        }
	}

	[ContextMenu("Pick")]
    public void Pick() {
        if (pulledUp && !removedFromPlot) {
            rb.useGravity = true;
            rb.isKinematic = false;
            col.isTrigger = false;
            removedFromPlot = true;
            if(plotIn != null) {
                plotIn.SquishBeetles();
                plotIn.RemovePlant();
            }

            TimeManager.instance.AddGarbage(gameObject);
        }
    }

    [ContextMenu("Drop")]
    public void Drop() {
        if(removedFromPlot) {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }
}
