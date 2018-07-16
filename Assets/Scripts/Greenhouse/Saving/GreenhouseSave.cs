using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GreenhouseSave {
    [SerializeField] public int previousDay;
    [SerializeField] public SavePlot[] plots;
    //TODO: add saving of the state of out/inboxes.
}
