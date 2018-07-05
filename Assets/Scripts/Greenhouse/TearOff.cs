using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearOff : MonoBehaviour
{

	public Letter letter;

	private void OnJointBreak(float breakForce)
	{
		letter.JointBroke();
	}
}
