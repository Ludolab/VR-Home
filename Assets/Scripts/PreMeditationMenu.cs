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
    private float FADE_TIME = 0.5f; //TODO: add fading transitions between menu parts

    private void Start()
    {
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
        if(timeSelect.IsActive()) {
            if(timeSelect.AnyTogglesOn()) {
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
        }
        if (environSelect.IsActive())
        {
            if (environSelect.AnyTogglesOn())
            {
                //Set-up, to potentially add different environments user can choose from.
                MUserSettings.setEnviron(environSelect.ActiveToggles().FirstOrDefault().name);

                this.gameObject.transform.Find("Environment").gameObject.SetActive(false);

                SceneManager.LoadScene(nextScene);
            }
        }
	}
}
