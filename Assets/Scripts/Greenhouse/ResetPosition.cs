using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{

	private Vector3 initialPos;
	private Quaternion initialRot;

	private void Start()
	{
		initialPos = transform.position;
		initialRot = transform.rotation;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Reset();
		}
	}

	private void Reset()
	{
		transform.position = initialPos;
		transform.rotation = initialRot;
	}
}
