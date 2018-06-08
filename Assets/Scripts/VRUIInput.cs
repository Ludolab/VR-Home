using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(SteamVR_LaserPointer))]
public class VRUIInput : MonoBehaviour
{
    private SteamVR_LaserPointer laserPointer;
    private SteamVR_TrackedController trackedController;

    private void OnEnable()
    {
        laserPointer = GetComponent<SteamVR_LaserPointer>();
        laserPointer.PointerIn -= HandlePointerIn;
        laserPointer.PointerIn += HandlePointerIn;
        laserPointer.PointerOut -= HandlePointerOut;
        laserPointer.PointerOut += HandlePointerOut;

        trackedController = GetComponent<SteamVR_TrackedController>();
        if (trackedController == null)
        {
            trackedController = GetComponentInParent<SteamVR_TrackedController>();
        }
        trackedController.TriggerClicked -= HandleTriggerClicked;
        trackedController.TriggerClicked += HandleTriggerClicked;
    }

    //Perform action when an object is selected by a laser and trigger is clicked.
    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }
    }

    private void HandlePointerIn(object sender, PointerEventArgs e)
    {
        //Perform button select.
        Button button = e.target.GetComponent<Button>();
        if (button != null)
        {
            button.Select();
        }
        //Alternatively, perform toggle select.
        Toggle toggle = e.target.GetComponent<Toggle>();
        if (toggle != null)
        {
            toggle.Select();
            e.target.gameObject.GetComponentInChildren<Text>().gameObject.AddComponent<Outline>();
        }
    }

    //Deselect object when laser leaves collider range.
    private void HandlePointerOut(object sender, PointerEventArgs e)
    {
        Button button = e.target.GetComponent<Button>();
        if (button != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        Toggle toggle = e.target.GetComponent<Toggle>();
        if (toggle != null)
        {
            Outline highlight = e.target.gameObject.GetComponentInChildren<Text>().gameObject.GetComponent<Outline>();
            if(highlight != null) Destroy(highlight);
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}