using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bowl : MonoBehaviour {

    protected int numObjects = 0;

    public void AddObject()
    {
        numObjects++;
        print("Objects: " + numObjects);
        Refresh();
    }

    public void RemoveObject()
    {
        numObjects--;
        print("Objects: " + numObjects);
        Refresh();
    }

    protected abstract void Refresh();
}
