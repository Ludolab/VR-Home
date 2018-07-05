using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

	public GameObject neighborsObj;

	private int day = 0;

	private Neighbor[] neighbors;
	
	private void Start()
	{
		neighbors = neighborsObj.GetComponentsInChildren<Neighbor>();

		ProcessDay();
	}

	private void ProcessDay()
	{
		//TODO: grow plants (based on day % 2, set half the fruit spots to ripe and the other half to unripe (if picked))
		//TODO: clear outbox
		//TODO: tell neighbors about outbox contents
		foreach (Neighbor neighbor in neighbors)
		{
			neighbor.StartDay(day);
		}
	}

	[ContextMenu("Next Day")]
	public void NextDay()
	{
		day++;

		ProcessDay();
	}
}
