using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : PickUp
{

    public Vector3 minPoint;
    public Vector3 maxPoint;

    private bool isHeld;

    private SteamVR_TrackedObject holder;

    private Vector3 offset;
    private Quaternion initialRotation;

    private new void Start()
    {
        base.Start();
        isHeld = false;
        gameObject.transform.position = minPoint;
    }

    private new void Update()
    {
        base.Update();
        if (isHeld)
        {
            gameObject.transform.position = getClosestPointOnLine(getFollowedPoint());
        }
    }

    protected override void AttachToController(SteamVR_TrackedObject controller)
    {
        holder = controller;
        offset = gameObject.transform.position - controller.transform.position;
        initialRotation = controller.transform.rotation;
        isHeld = true;
    }

    protected override void ReleaseFromController(SteamVR_TrackedObject controller)
    {
        isHeld = false;
    }

    private Vector3 getClosestPointOnLine(Vector3 trackingPoint)
    {
        Vector3 line = minPoint - maxPoint;
        line.Normalize();//this needs to be a unit vector
        Vector3 v = trackingPoint - minPoint;
        float d = Vector3.Dot(v, line);
        Vector3 closest = minPoint + (line * d);
        if (minPoint.x < maxPoint.x)
        {
            if (closest.x < minPoint.x)
            {
                closest = minPoint;
            }
            return closest;
        }
        else if (maxPoint.x < minPoint.x)
        {
            if (closest.x < maxPoint.x)
            {
                closest = maxPoint;
            }
            return closest;
        }
        // Anything below this only apples if the points have the same x-coordinate
        else if (minPoint.y < maxPoint.y)
        {
            if (closest.y < minPoint.y)
            {
                closest = minPoint;
            }
            return closest;
        }
        else if (maxPoint.y < minPoint.y)
        {
            if (closest.y < maxPoint.y)
            {
                closest = maxPoint;
            }
            return closest;
        }
        // Anything below this only apples if the points have the same x-coordinate AND the same y-coordinate
        else if (minPoint.z < maxPoint.z)
        {
            if (closest.z < minPoint.z)
            {
                closest = minPoint;
            }
            return closest;
        }
        else if (maxPoint.z < minPoint.z)
        {
            if (closest.z < maxPoint.z)
            {
                closest = maxPoint;
            }
            return closest;
        }
        else
        {
            return minPoint;
        }
    }

    private Vector3 getFollowedPoint()
    {
        Quaternion currentRotation = holder.transform.rotation;
        Quaternion rotationDifference = Quaternion.Inverse(initialRotation) * currentRotation;
        Vector3 newOffset = rotationDifference * offset;
        return (holder.transform.position + newOffset);
    }
}
