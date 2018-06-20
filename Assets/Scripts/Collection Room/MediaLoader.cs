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
    public SaveLoadMedia loadMedia;

    private void Start()
    {
        float height = transform.position.y;

        Texture2D[] textures = Resources.LoadAll<Texture2D>("Media");
        foreach (Texture2D tex in textures)
        {
            GameObject photo = Instantiate(photoPrefab);
            CollectionData.addImage(photo);
            photo.GetComponent<ImageQuad>().SetTexture(tex);
            photo.transform.position = new Vector3(transform.position.x, height, transform.position.z);
            height += HEIGHT_DIFF;
        }

        VideoClip[] videos = Resources.LoadAll<VideoClip>("Media");
        foreach (VideoClip vid in videos)
        {
            GameObject video = Instantiate(videoPrefab);
            CollectionData.addImage(video);
            video.GetComponent<VideoQuad>().SetVideo(vid);
            video.transform.position = new Vector3(transform.position.x, height, transform.position.z);
            height += HEIGHT_DIFF;
        }

        AudioClip[] audios = Resources.LoadAll<AudioClip>("Media");
        foreach (AudioClip ac in audios)
        {
            GameObject audio = Instantiate(audioPrefab);
            CollectionData.addSound(audio);
            audio.GetComponent<Record>().SetAudio(ac);
            audio.transform.position = new Vector3(transform.position.x, height, transform.position.z);
            height += HEIGHT_DIFF;
        }

        loadMedia.Load();
    }
}
