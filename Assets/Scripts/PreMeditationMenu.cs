using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class PreMeditationMenu : MonoBehaviour
{

    public Button okButton;
    public ToggleGroup timeSelect; //Possible times.
    public ToggleGroup environSelect; //Possible environments.
    public string nextScene;

    private void Start()
    {
        okButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        string timeActive = timeSelect.ActiveToggles().FirstOrDefault().name;
        switch (timeActive)
        {
            case "5":
                MUserSettings.setTime(300f);
                break;
            case "10":
                MUserSettings.setTime(600f);
                break;
            case "20":
                MUserSettings.setTime(1200f);
                break;
            case "inf":
                MUserSettings.setTime(-1f); //Use negative value to indicate infinite time.
                break;
        }

        //Set-up, to potentially add different environments user can choose from.
        MUserSettings.setEnviron(environSelect.ActiveToggles().FirstOrDefault().name);

        SceneManager.LoadScene(nextScene);
    }
}
