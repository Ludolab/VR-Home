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
        if (plotIn != null
            && other.gameObject.name == "Surface Collider")
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            col.isTrigger = false;
            pulledUp = true;
            plotIn.SquishBeetles();
            plotIn.RemovePlant();

            TimeManager.instance.AddGarbage(gameObject);
        }
	}

    [ContextMenu("Drop")]
    public void Drop() {
        if(pulledUp == true) {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }
}
