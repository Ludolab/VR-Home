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
        col.isTrigger = false;
    }

    public void SetPlot(Plot plot)
    {
        plotIn = plot;
    }

	private void OnCollisionExit(Collision other)
	{
        Debug.Log("Exiting collision");
        if (plotIn != null
            && other.gameObject.name == "Surface Collider")
        {
            Debug.Log("Removing plant");
            plotIn.SquishBeetles();
            rb.useGravity = true;
            rb.isKinematic = false;
            pulledUp = true;
            plotIn.RemovePlant();

            TimeManager.instance.AddGarbage(gameObject);
        }
	}

    [ContextMenu("Drop")]
    public void Drop() {
        rb.useGravity = true;
        rb.isKinematic = false;
    }
}
