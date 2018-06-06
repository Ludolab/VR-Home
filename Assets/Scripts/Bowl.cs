using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : MonoBehaviour {

    private int numObjects = 0;

    private void Start()
    {
        
    }

    public void AddObject()
    {
        numObjects++;
        print("Objects: " + numObjects);
    }

    public void RemoveObject()
    {
        numObjects--;
        print("Objects: " + numObjects);
    }
}
