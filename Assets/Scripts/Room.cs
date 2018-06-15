﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    [SerializeField] public string user;
    //[SerializeField] public List<GameObject> objInRoom;
    [SerializeField] public SaveTransform[] objectTransforms;
}