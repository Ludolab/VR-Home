using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giftable : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Outbox"))
		{
			Outbox o = other.transform.parent.GetComponent<Outbox>();
			o.AddGift(this);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Outbox"))
		{
			Outbox o = other.transform.parent.GetComponent<Outbox>();
			o.RemoveGift(this);
		}
	}
}
