using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreEnterMenu : MonoBehaviour {

    public GameObject enterSequence; //ritual for entry to enter room.
    public GameObject room; //room to be generated; could be extended to a list of possible environments/rooms.
    public GameObject start; //space the menu first is created in.
    public Button okButton;

	private void Start()
    {
        okButton.onClick.AddListener(OnClick);
	}

    private void OnClick() {
        room.SetActive(true); //or Instantiate(room);
        enterSequence.GetComponent<SitDown>().start = start;
        enterSequence.SetActive(true); //or Instantiate(enterSequence);
    }
}
