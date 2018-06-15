using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveTransform {
    [SerializeField] public string objName;
    [SerializeField] public float xPosition;
    [SerializeField] public float yPosition;
    [SerializeField] public float zPosition;
    [SerializeField] public float xScale;
    [SerializeField] public float yScale;
    [SerializeField] public float zScale;
}
