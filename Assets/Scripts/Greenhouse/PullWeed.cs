using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullWeed : MonoBehaviour
{

	public AudioClip popOutClip;

	public float pullDistance;

	public GameObject dragObj;
	public GameObject modelObj;

	private InteractionBehaviour ib;
	private Rigidbody rb;
	private Collider col;
	private AudioSource audioSrc;
	private Rigidbody dragRB;

	private bool grasped = false;
	private bool pulled = false;
	private Vector3 basePosition;
	private Quaternion baseRotation;
	private Vector3 offset;
	private Vector3 startScale;
	private Vector3 grabbedPosition;

	private void Start()
	{
		ib = modelObj.GetComponent<InteractionBehaviour>();
		rb = modelObj.GetComponent<Rigidbody>();
		dragRB = dragObj.GetComponent<Rigidbody>();
		col = modelObj.GetComponent<Collider>();
		audioSrc = GetComponent<AudioSource>();
		startScale = modelObj.transform.localScale;
	}

	public void OnGrasp()
	{
		grasped = true;
		basePosition = dragObj.transform.position;
		baseRotation = dragObj.transform.rotation;
		offset = modelObj.transform.position - basePosition;
		grabbedPosition = modelObj.transform.position;
		//TODO: could play some sort of looping stretchy sound here?
	}

	private void FixedUpdate()
	{
		if (grasped)
		{
			Vector3 dragPosition = dragObj.transform.position;
			//Quaternion dragRotation = dragObj.transform.rotation;
			//Vector3 dragScale = dragObj.transform.localScale;

			//TODO: stretch/turn around anchor point in roots
			Vector3 diff = dragPosition - basePosition;
			float dist = diff.magnitude;
			//print("base: " + basePosition + ", new: " + newPosition + ", dist: " + dist);

			float yScale = startScale.y + dist * 4;
			modelObj.transform.localScale = new Vector3(startScale.x, yScale, startScale.z);

			//float yAvg = ((dragPosition - basePosition) / 2).y;
			//transform.position = yAvg * Vector3.up + offset + basePosition;

			//TODO: rotation!
			/*Vector3 from = basePosition - transform.position;
			Vector3 to = newPosition - transform.position;
			transform.rotation = Quaternion.FromToRotation(from, to);*/
			modelObj.transform.LookAt(dragObj.transform);
			modelObj.transform.Rotate(90, 0, 0);

			//TODO: adjust pitch of stretchy sound?

			/*dragObj.transform.position = dragPosition;
			dragObj.transform.rotation = dragRotation;
			dragObj.transform.localScale = dragScale;*/

			if (dist > pullDistance)
			{
				modelObj.transform.localScale = startScale;
				print("POP");
				PullOut();
			}
		}
	}

	private void PullOut()
	{
		audioSrc.PlayOneShot(popOutClip);
		dragObj.SetActive(false);
		col.enabled = true;
		rb.isKinematic = false;
		rb.useGravity = true;
		ib.enabled = true;
		//TODO: grasp, snap into hand
		//(might not work right if OnUngrasp() gets called when dragObj is disabled)
		//TODO: stop stretchy sound??? may not be necessary if OnUngrasp() gets called when dragObj is disabled
	}

	public void OnUngrasp()
	{
		grasped = false;
		modelObj.transform.position = grabbedPosition;
		modelObj.transform.localScale = startScale;
		dragObj.transform.position = basePosition;
		dragObj.transform.rotation = baseRotation;
		//TODO: stop stretchy sound?
	}

	public void OnContact()
	{
		if (!grasped && !pulled)
		{
			//TODO: wobble
		}
	}
}
