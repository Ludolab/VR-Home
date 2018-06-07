using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTimer : MonoBehaviour {

    public int time; //in seconds.
    public GameObject musicSource;
    public GameObject soundSource;

    private int timeLeft;
    private AudioSource musicAudio;
    private AudioSource soundAudio;
    private bool startFade;
    private float FADE_SPEED = 0.5f;

	void Start () {
        musicAudio = musicSource.GetComponent<AudioSource>();
        soundAudio = soundSource.GetComponent<AudioSource>();
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
        while(musicAudio.volume > 0 || soundAudio.volume > 0) {
            yield return new WaitForSeconds(0.01f);
            if(musicAudio.volume > 0) musicAudio.volume -= 0.01f * FADE_SPEED;
            if(soundAudio.volume > 0) soundAudio.volume -= 0.01f * FADE_SPEED;
        }
        //Remove components since no more sound will be playing for the rest of the time.
        Destroy(musicSource);
        Destroy(soundSource);
    }
	
	
}
