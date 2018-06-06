using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PreEnterMenu : MonoBehaviour {

    public Button okButton;
    public string nextScene; //

	private void Start()
    {
        okButton.onClick.AddListener(OnClick);
	}

    private void OnClick() {
        SceneManager.LoadScene(nextScene);
        //TODO: add integration that will load the scene with a timer on the sound, based on drop-down menu.
    }
}
