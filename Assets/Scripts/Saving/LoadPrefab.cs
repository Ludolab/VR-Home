using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LoadPrefab {
    [SerializeField] public string prefabToLoad;
    [SerializeField] public SaveObject objData;
    [SerializeField] public SaveObject[] childrenData;
}
