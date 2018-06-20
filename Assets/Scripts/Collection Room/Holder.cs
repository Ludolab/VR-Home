using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {

    }

    public virtual void Apply(GameObject obj)
    {
        GetComponent<Renderer>().material.mainTexture = obj.GetComponent<Renderer>().material.mainTexture;
    }
}
