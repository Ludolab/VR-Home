using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTimer : MonoBehaviour {

    public int time; //in seconds.
    public float fadeSpeed; //how fast audio should fade out.
    public GameObject musicSource;
    public GameObject soundSource;

    private int timeLeft;
    private AudioSource musicAudio;
    private AudioSource soundAudio;
    private bool startFade;

	// Use this for initialization
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
            Debug.Log(timeLeft);
        }
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        while(musicAudio.volume > 0 || soundAudio.volume > 0) {
            yield return new WaitForSeconds(0.005f);
            if(musicAudio.volume > 0) musicAudio.volume -= 0.005f * fadeSpeed;
            if(soundAudio.volume > 0) soundAudio.volume -= 0.005f * fadeSpeed;

        }
        Destroy(musicSource);
        Destroy(soundSource);
    }
	
	
}
