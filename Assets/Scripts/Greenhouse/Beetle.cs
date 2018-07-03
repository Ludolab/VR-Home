using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beetle : MonoBehaviour
{

	private void OnCollisionEnter(Collision collision)
	{
		print("COLLISION ENTER " + collision.gameObject);
	}

	private void OnCollisionStay(Collision collision)
	{
		print("COLLISION STAY " + collision.gameObject);
	}
}
