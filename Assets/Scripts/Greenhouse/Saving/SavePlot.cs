using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavePlot {
    [SerializeField] public ID plotID;
    [SerializeField] public string plant; //if plant == "", then no plant.
    [SerializeField] public int plantDayBorn;
    [SerializeField] public int plantStage;
    [SerializeField] public bool watered;
    [SerializeField] public SaveObject[] weeds;
    [SerializeField] public int[] beetles;
    [SerializeField] public int[] fruits;
}
