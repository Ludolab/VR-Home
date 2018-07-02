using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{

    private ConfigurableJoint joint;
    private InteractionBehaviour ib;

    private void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
        ib = GetComponent<InteractionBehaviour>();
    }

    public void Release()
    {
        Destroy(joint);
        ib.RefreshPositionLockedState();
    }
}
