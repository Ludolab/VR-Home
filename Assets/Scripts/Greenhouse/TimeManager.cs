using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

	private int day = 0;
	
	private void Start()
	{

	}

	public void NextDay()
	{
		day++;

		//TODO: grow plants (based on day % 2, set half the fruit spots to ripe and the other half to unripe (if picked))
		//TODO: clear outbox
		//TODO: spawn gifts based on day & outbox contents
	}
}
