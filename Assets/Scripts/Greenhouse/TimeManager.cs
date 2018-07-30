﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

	public static TimeManager instance;

    public SaveLoadGreenhouse saveLoad;
	public GameObject neighborsObj;
	public Outbox[] outboxes;
    public Plot[] plots;

	private int day = 0;

	private Neighbor[] neighbors;
	//private List<Plant> plants = new List<Plant>();
	private List<GameObject> garbage = new List<GameObject>();
    private List<Starter> seedStarters = new List<Starter>();

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
        if(!saveLoad.loadPrevious) ProcessDay();
	}

    public void ProcessDay()
	{
		print("Day " + day);
		foreach (Outbox outbox in outboxes)
		{
			string[] contents = outbox.ClearFruit();
			if (contents.Length > 0)
			{
				outbox.GetOwner().GiveGift(contents);
			}
		}

		foreach (GameObject obj in garbage)
		{
			Destroy(obj);
		}
		garbage.Clear();

        NeighborStart(true);

		foreach (Plot plot in plots)
		{
            plot.StartDay();
		}
	}

    public void NeighborStart(bool initializeDay) {
        foreach (Neighbor neighbor in neighbors)
        {
            neighbor.StartDay(day, initializeDay);
        }
    }

    public void SetDay(int d)
    {
        day = d;
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
	
	public void AddGarbage(GameObject obj)
	{
		garbage.Add(obj);
	}

    public void AddStarter(Starter starter)
    {
        seedStarters.Add(starter);
    }

    public void RemoveStarter(Starter starter)
    {
        seedStarters.Remove(starter);
    }

    public Starter[] GetStarters()
    {
        return seedStarters.ToArray();
    }
}
