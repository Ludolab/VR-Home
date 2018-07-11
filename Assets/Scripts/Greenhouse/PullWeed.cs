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
	private InteractionBehaviour dragIB;

	private bool grasped = false;
	private bool pulled = false;
	private Vector3 basePosition;
	private Vector3 baseGrabPosition;
	private Quaternion baseRotation;
	private Vector3 startScale;
	private Quaternion startRotation;
	private Vector3 grabbedPosition;

	private void Start()
	{
		ib = modelObj.GetComponent<InteractionBehaviour>();
		rb = modelObj.GetComponent<Rigidbody>();
		dragRB = dragObj.GetComponent<Rigidbody>();
		dragIB = dragObj.GetComponent<InteractionBehaviour>();
		col = modelObj.GetComponent<Collider>();
		audioSrc = GetComponent<AudioSource>();
		startScale = modelObj.transform.localScale;
		startRotation = modelObj.transform.rotation;
	}

	public void OnGrasp()
	{
		grasped = true;
		basePosition = dragObj.transform.position;
		baseGrabPosition = GetDragPosition();
		baseRotation = dragObj.transform.rotation;
		grabbedPosition = modelObj.transform.position;
		//TODO: could play some sort of looping stretchy sound here?
	}

	private Vector3 GetDragPosition()
	{
		return dragIB.GetGraspPoint(dragIB.graspingController);
	}

	private void FixedUpdate()
	{
		if (grasped)
		{
			Vector3 dragPosition = GetDragPosition();

			Vector3 diff = dragPosition - baseGrabPosition;
			float dist = diff.magnitude;

			float yScale = startScale.y + dist * 4;
			modelObj.transform.localScale = new Vector3(startScale.x, yScale, startScale.z);

			modelObj.transform.LookAt(dragObj.transform);
			modelObj.transform.Rotate(90, 0, 0);

			float yRot = modelObj.transform.localEulerAngles.y;
			modelObj.transform.Rotate(0, -yRot, 0, Space.Self);

			//TODO: adjust pitch of stretchy sound?
			
			if (dist > pullDistance)
			{
				modelObj.transform.localScale = startScale;
				print("PULL OUT");
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

		List<InteractionController> controllers = new List<InteractionController>
		{
			dragIB.graspingController
		};
		ib.BeginGrasp(controllers);
		//TODO: snap into hand
		//(might not work right if OnUngrasp() gets called when dragObj is disabled)
		//TODO: stop stretchy sound??? may not be necessary if OnUngrasp() gets called when dragObj is disabled
	}

	public void OnUngrasp()
	{
		print("UNGRASP");
		grasped = false;
		modelObj.transform.position = grabbedPosition;
		modelObj.transform.localScale = startScale;
		modelObj.transform.rotation = startRotation;
		dragObj.transform.position = basePosition;
		dragObj.transform.rotation = baseRotation;
		//TODO: stop stretchy sound?
	}

	public void OnContact()
	{
		if (!grasped && !pulled)
		{
			//TODO: wobble
			//TODO: play wobble sound
			//TODO: make this the case for all plants?
		}
	}
}
