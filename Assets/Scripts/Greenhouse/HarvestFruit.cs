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
           && other == plotIn.myDirt.SurfaceCollider.GetComponent<Collider>())
        {
            plotIn.SquishBeetles();
            col.isTrigger = false;
            rb.useGravity = true;
            rb.isKinematic = false;
            pulledUp = true;
            plotIn.RemovePlant();

            TimeManager.instance.AddGarbage(gameObject);
        }
    }

	private void OnCollisionExit(Collision other)
	{
        Debug.Log("Detected collision exit.");
        if (other.gameObject.name.StartsWith("Contact") && pulledUp == true)
        {
            Debug.Log("Detected exiting leap hand.");
            rb.isKinematic = false;
        }
	}

}
