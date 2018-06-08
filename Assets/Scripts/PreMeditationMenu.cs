using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class PreMeditationMenu : MonoBehaviour
{

    public ToggleGroup timeSelect; //Possible times.
    public ToggleGroup environSelect; //Possible environments.
    public string nextScene;

    private float WELCOME_LOAD_TIME = 2f;

    private void Start()
    {
        // Make sure the scene starts with only the "Welcome" text and menu backdrop.
        foreach(Transform child in transform) {
            child.gameObject.SetActive(false);
        }
        transform.Find("Background").gameObject.SetActive(true);
        transform.Find("Welcome").gameObject.SetActive(true);

        // Start option-select sequence.
        StartCoroutine(Welcome());
    }

    private IEnumerator Welcome()
    {
        yield return new WaitForSeconds(WELCOME_LOAD_TIME);

        this.gameObject.transform.Find("Welcome").gameObject.SetActive(false);
        this.gameObject.transform.Find("Time").gameObject.SetActive(true);
    }

	private void Update()
	{
        // Check if the user has selected a time, and then store that for room generation.
        if (timeSelect.AnyTogglesOn()) setTime();

        // Check if the user has selected an environment, and then store that for room generation.
        if (environSelect.AnyTogglesOn()) setEnvironment();
	}

    private void setTime()
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

        this.gameObject.transform.Find("Time").gameObject.SetActive(false);
        this.gameObject.transform.Find("Environment").gameObject.SetActive(true);
    }

    private void setEnvironment()
    {
        //Set-up, to potentially add different environments user can choose from.
        MUserSettings.setEnviron(environSelect.ActiveToggles().FirstOrDefault().name);

        this.gameObject.transform.Find("Environment").gameObject.SetActive(false);

        //Move into the room, since set-up is complete.
        SceneManager.LoadScene(nextScene);
    }

}
