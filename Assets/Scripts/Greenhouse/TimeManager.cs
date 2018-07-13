using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

	public static TimeManager instance;

	public GameObject neighborsObj;
	public Outbox[] outboxes;
    public Plot[] plots;

	private int day = 0;

	private Neighbor[] neighbors;
	//private List<Plant> plants = new List<Plant>();
	private List<GameObject> garbage = new List<GameObject>();

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("Should not have more than one TimeManager in a scene!");
		}
		instance = this;
	}

	private void Start()
	{
		neighbors = neighborsObj.GetComponentsInChildren<Neighbor>();
		ProcessDay();
	}

	private void ProcessDay()
	{
		print("Day " + day);
		foreach (Outbox outbox in outboxes)
		{
			string[] contents = outbox.ClearFruit();
			if (contents.Length > 0)
			{
				string name = outbox.GetLabel();
				if (name != null)
				{
					Neighbor receiver = GetNeighborByName(name);
					receiver.GiveGift(contents);
				}
			}

			if (day == 1)
			{
				outbox.gameObject.SetActive(true);
			}
		}

		foreach (GameObject obj in garbage)
		{
			Destroy(obj);
		}
		garbage.Clear();

		foreach (Neighbor neighbor in neighbors)
		{
			neighbor.StartDay(day);
		}

		foreach (Plot plot in plots)
		{
			plot.StartDay(day);
		}
	}

	public int GetDay()
	{
		return day;
	}

	[ContextMenu("Next Day")]
	public void NextDay()
	{
		day++;

		ProcessDay();
	}

	/*public void AddPlant(Plant plant)
	{
		plants.Add(plant);
	}*/

	public void AddOutboxLabel(NeighborInfo info)
	{
		foreach (Outbox outbox in outboxes)
		{
			outbox.AddLabel(info);
		}
	}

	private Neighbor GetNeighborByName(string name)
	{
		return neighbors.First(n => n.info.name == name);
	}

	public void AddGarbage(GameObject obj)
	{
		garbage.Add(obj);
	}
}
