using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullWeed : MonoBehaviour
{

	public AudioClip popOutClip;

	public float pullDistance;

	private InteractionBehaviour ib;
	private Rigidbody rb;
	private AudioSource audioSrc;

	private bool grasped = false;
	private bool pulled = false;
	private Vector3 basePosition;
	private Vector3 offset;
	private Vector3 startScale;

	private void Start()
	{
		ib = GetComponent<InteractionBehaviour>();
		rb = GetComponent<Rigidbody>();
		audioSrc = GetComponent<AudioSource>();
		startScale = transform.localScale;
	}

	public void OnGrasp()
	{
		grasped = true;
		basePosition = GetHandPosition();
		offset = transform.position - basePosition;
		print("GRASP");
	}

	private Vector3 GetHandPosition()
	{
		foreach (InteractionHand hand in ib.graspingHands)
		{
			return hand.transform.position;
		}
		print("No grasping hands...");
		return Vector3.zero;
	}

	public void HoldGrasp()
	{
		//TODO: could play some sort of stretchy sound here?

		Vector3 newPosition = GetHandPosition();

		//TODO: stretch/turn around anchor point in roots
		Vector3 diff = newPosition - basePosition;
		float dist = diff.magnitude;
		print("base: " + basePosition + ", new: " + newPosition + ", dist: " + dist);

		float yScale = startScale.y + dist;
		transform.localScale = new Vector3(startScale.x, yScale, startScale.z);

		transform.position = (newPosition + basePosition) / 2 + offset;

		/*Vector3 from = basePosition - transform.position;
		Vector3 to = newPosition - transform.position;
		transform.rotation = Quaternion.FromToRotation(from, to);*/

		if (dist > pullDistance)
		{
			transform.localScale = startScale;
			print("POP");
			PullOut();
		}
	}

	private void PullOut()
	{
		audioSrc.PlayOneShot(popOutClip);
		rb.isKinematic = false;
		rb.useGravity = true;
		ib.moveObjectWhenGrasped = true;
		ib.ReleaseFromGrasp(); //TODO no
		//ib.RefreshPositionLockedState(); //??
		//TODO: snap into hand?
	}

	public void OnUngrasp()
	{
		grasped = false;
	}

	public void OnContact()
	{
		if (!grasped && !pulled)
		{
			//TODO: wobble
		}
	}
}
