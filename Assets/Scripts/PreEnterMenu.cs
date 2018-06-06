using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class PreEnterMenu : MonoBehaviour {

    public Button okButton;
    public ToggleGroup timeSelect;
    public string nextScene;

	private void Start()
    {
        okButton.onClick.AddListener(OnClick);
	}

    private void OnClick() {
        string timeActive = timeSelect.ActiveToggles().FirstOrDefault().name;
        switch(timeActive)
        {
            case "5":
                UserSettings.setTime(300f);
                break;
            case "10":
                UserSettings.setTime(600f);
                break;
            case "20":
                UserSettings.setTime(1200f);
                break;
            case "inf":
                UserSettings.setTime(-1f); //TODO: use the negative value to indicate to room set-up script to run infinitely (i.e. do not create SoundTimer object).
                break;
        }

        Debug.Log(UserSettings.getTime());
        
        SceneManager.LoadScene(nextScene);
    }
}
