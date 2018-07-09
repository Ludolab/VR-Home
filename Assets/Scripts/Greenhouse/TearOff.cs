using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearOff : MonoBehaviour
{

	public Envelope envelope;

	private void OnJointBreak(float breakForce)
	{
		envelope.JointBroke();
	}
}
