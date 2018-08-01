using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GreenhouseSave {
    [SerializeField] public int previousDay;
    [SerializeField] public SavePlot[] plots;
    [SerializeField] public SaveOutbox[] outboxes;
    [SerializeField] public SaveStarter[] seedStarters;
    //TODO: add saving of seeds in seed tray.
}

[System.Serializable]
public class SavePlot
{
    [SerializeField] public ID plotID;
    [SerializeField] public string plant; // If plant == "", then no plant.
    [SerializeField] public int plantDayBorn;
    [SerializeField] public int plantStage;
    [SerializeField] public float watered;
    [SerializeField] public SaveObject[] weeds;
    [SerializeField] public int[] beetles; // Save beetles by their transform position IDs.
    [SerializeField] public int[] fruits; // Save fruits by their transform position IDs.
}

[System.Serializable]
public class SaveOutbox
{
    [SerializeField] public string neighbor;
    [SerializeField] public SaveGift[] givenGifts; //What the player has put into the box.
}

[System.Serializable]
public class SaveStarter
{
    [SerializeField] public string plantType;
    [SerializeField] public float xPos;
    [SerializeField] public float yPos;
    [SerializeField] public float zPos;
}


[System.Serializable]
public class SaveGift
{
    [SerializeField] public string gift;
    [SerializeField] public SaveObject giftObject;
}