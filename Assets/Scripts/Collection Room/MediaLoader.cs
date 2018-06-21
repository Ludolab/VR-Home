using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MediaLoader : MonoBehaviour
{

    private const float HEIGHT_DIFF = 0.05f;

    public GameObject photoPrefab;
    public GameObject videoPrefab;
    public GameObject audioPrefab;

    private void Start()
    {
        float height = transform.position.y;

        Texture2D[] textures = Resources.LoadAll<Texture2D>("Media");
        foreach (Texture2D tex in textures)
        {
            GameObject photo = Instantiate(photoPrefab);
            photo.GetComponent<ImageQuad>().SetTexture(tex);
            photo.transform.position = new Vector3(transform.position.x, height, transform.position.z);
            height += HEIGHT_DIFF;
            CollectionData.addToImages(tex.name, photo);
        }

        VideoClip[] videos = Resources.LoadAll<VideoClip>("Media");
        foreach (VideoClip vid in videos)
        {
            GameObject video = Instantiate(videoPrefab);
            video.GetComponent<VideoQuad>().SetVideo(vid);
            video.transform.position = new Vector3(transform.position.x, height, transform.position.z);
            height += HEIGHT_DIFF;
            CollectionData.addToVideos(vid.name, video);
        }

        AudioClip[] audios = Resources.LoadAll<AudioClip>("Media");
        foreach (AudioClip ac in audios)
        {
            GameObject audio = Instantiate(audioPrefab);
            audio.GetComponent<Record>().SetAudio(ac);
            audio.transform.position = new Vector3(transform.position.x, height, transform.position.z);
            height += HEIGHT_DIFF;
            CollectionData.addToSounds(ac.name, audio);
        }


        gameObject.GetComponent<SaveLoad>().Load();
        gameObject.GetComponent<SaveLoadMedia>().Load();
    }
}
