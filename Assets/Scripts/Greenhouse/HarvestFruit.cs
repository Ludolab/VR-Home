using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestFruit : MonoBehaviour {

    private bool noBeetles;
    private Plot plotIn;
    private Rigidbody rb;
    private Collider col;
    private bool pulledUp = false;

    private bool rbikPrevFrame;

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
            plotIn.SquishBeetles();
            col.isTrigger = false;
            rb.useGravity = true;
            pulledUp = true;
            plotIn.RemovePlant();

            Debug.Log(rb.isKinematic);

            TimeManager.instance.AddGarbage(gameObject);
        }
        if(other.gameObject.name.StartsWith("Contact") && pulledUp == true) {
            rb.isKinematic = false;
        }
	}



	private void Update()
	{
        if (rbikPrevFrame == false && rb.isKinematic == true) Debug.Log("rb activated again");
        rbikPrevFrame = rb.isKinematic;
	}
}
