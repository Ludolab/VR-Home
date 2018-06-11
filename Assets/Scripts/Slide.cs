using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : PickUp
{


    public GameObject Sun;

    private SetSky skyScript;

    public Vector3 minPoint; //in local coordinates
    public Vector3 maxPoint; //in local coordinates

    private Vector3 minPointGlobal;
    private Vector3 maxPointGlobal;

    private bool isHeld;

    private SteamVR_TrackedObject controllerHolder;

    private Vector3 offset;
    private Quaternion initialRotation;

    protected override void Start()
    {
        base.Start();
        skyScript = Sun.GetComponent<SetSky>();
        isHeld = false;
        gameObject.transform.localPosition = minPoint;
        minPointGlobal = gameObject.transform.TransformPoint(minPoint);
        maxPointGlobal = gameObject.transform.TransformPoint(maxPoint);
    }

    protected override void Update()
    {
        base.Update();
        if (isHeld)
        {
            gameObject.transform.position = getClosestPointOnLine(getFollowedPoint()); //in global coordinates
        }
        float totalDist = Vector3.Distance(minPoint, maxPoint);
        float partialDist = Vector3.Distance(minPoint, gameObject.transform.localPosition); //in local coordinates
        skyScript.percentThroughDay = (partialDist / totalDist) * 100;
    }

    protected override void AttachToController(SteamVR_TrackedObject controller)
    {
        controllerHolder = controller;
        offset = gameObject.transform.position - controller.transform.position; //in global coordinates
        initialRotation = controller.transform.rotation;
        isHeld = true;
    }

    protected override void ReleaseFromController(SteamVR_TrackedObject controller)
    {
        isHeld = false;
    }

    private Vector3 getClosestPointOnLine(Vector3 trackingPoint)
    {
        Vector3 line = minPointGlobal - maxPointGlobal;
        line.Normalize();//this needs to be a unit vector
        Vector3 v = trackingPoint - minPointGlobal;
        float d = Vector3.Dot(v, line);
        Vector3 closest = minPointGlobal + (line * d);
        if (minPointGlobal.x < maxPointGlobal.x)
        {
            if (closest.x < minPointGlobal.x)
            {
                closest = minPointGlobal;
            }
            if (closest.x > maxPointGlobal.x)
            {
                closest = maxPointGlobal;
            }
            return closest;
        }
        else if (maxPointGlobal.x < minPointGlobal.x)
        {
            if (closest.x < maxPointGlobal.x)
            {
                closest = maxPointGlobal;
            }
            if (closest.x > minPointGlobal.x)
            {
                closest = minPointGlobal;
            }
            return closest;
        }
        // Anything below this only apples if the points have the same x-coordinate
        else if (minPointGlobal.y < maxPointGlobal.y)
        {
            if (closest.y < minPointGlobal.y)
            {
                closest = minPointGlobal;
            }
            if (closest.y > maxPointGlobal.y)
            {
                closest = maxPointGlobal;
            }
            return closest;
        }
        else if (maxPointGlobal.y < minPointGlobal.y)
        {
            if (closest.y < maxPointGlobal.y)
            {
                closest = maxPointGlobal;
            }
            if (closest.y > minPointGlobal.y)
            {
                closest = minPointGlobal;
            }
            return closest;
        }
        // Anything below this only apples if the points have the same x-coordinate AND the same y-coordinate
        else if (minPointGlobal.z < maxPointGlobal.z)
        {
            if (closest.z < minPointGlobal.z)
            {
                closest = minPointGlobal;
            }
            if (closest.z > maxPointGlobal.z)
            {
                closest = maxPointGlobal;
            }
            return closest;
        }
        else if (maxPointGlobal.z < minPointGlobal.z)
        {
            if (closest.z < maxPointGlobal.z)
            {
                closest = maxPointGlobal;
            }
            if (closest.z > minPointGlobal.z)
            {
                closest = minPointGlobal;
            }
            return closest;
        }
        else
        {
            return minPointGlobal;
        }
    }

    private Vector3 getFollowedPoint()
    {
        Quaternion currentRotation = controllerHolder.transform.rotation;
        Quaternion rotationDifference = Quaternion.Inverse(initialRotation) * currentRotation;
        Vector3 newOffset = rotationDifference * offset;
        return (controllerHolder.transform.position + newOffset);
    }
}
