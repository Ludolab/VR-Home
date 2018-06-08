using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach to a game object with an audio source to have its audio fade out after a certain amount of time.
public class SoundTimer : MonoBehaviour {

    public int time; //in seconds.

    private int timeLeft;
    private AudioSource audio;
    private bool startFade;
    private float FADE_SPEED = 0.5f;

	void Start () {
        audio = this.GetComponent<AudioSource>();
        timeLeft = time;
        StartCoroutine(Countdown());
	}

    private IEnumerator Countdown()
    {
        while(timeLeft > 0) {
            yield return new WaitForSeconds(1.0f);
            timeLeft--;
        }
        //Fade music out once time is up.
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        //TODO: prevent user from being able to adjust volume at this point.
        while(audio != null && audio.volume > 0) {
            yield return new WaitForSeconds(0.01f);
            audio.volume -= 0.01f * FADE_SPEED;
        }
        //Remove audio component since no more sound will be playing for the rest of the time.
        if(audio != null) Destroy(audio);
    }

}
