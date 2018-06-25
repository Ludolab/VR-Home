using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MediaLoader : MonoBehaviour
{

    public Transform start; //should be a little bit along the string, since a photo will spawn here
    public Transform end;
    //TODO: support more than one string
    //TODO: support curved strings

    public Transform recordSpawn;
    public float recordHeight = 0.05f;

    public GameObject photoPrefab;
    public GameObject videoPrefab;
    public GameObject audioPrefab;

    private void Start()
    {
        //load photos along the string
        Vector3 startPos = start.position;
        Vector3 endPos = end.position;

        Texture2D[] textures = Resources.LoadAll<Texture2D>("Media");
        VideoClip[] videos = Resources.LoadAll<VideoClip>("Media");

        float i = 0;
        int numPhotos = textures.Length + videos.Length;

        foreach (Texture2D tex in textures)
        {
            GameObject photo = Instantiate(photoPrefab);
            photo.GetComponent<ImageQuad>().SetTexture(tex);
            photo.transform.position = Vector3.Lerp(startPos, endPos, i / numPhotos);
            photo.GetComponent<Rigidbody>().isKinematic = true;
            i++;
            CollectionData.addToImages(tex.name, photo);
        }

        foreach (VideoClip vid in videos)
        {
            GameObject video = Instantiate(videoPrefab);
            video.GetComponent<VideoQuad>().SetVideo(vid);
            video.transform.position = Vector3.Lerp(startPos, endPos, i / numPhotos);
            video.GetComponent<Rigidbody>().isKinematic = true;
            i++;
            CollectionData.addToVideos(vid.name, video);
        }

        //load records in a stack
        float height = recordSpawn.position.y;
        AudioClip[] audios = Resources.LoadAll<AudioClip>("Media");
        foreach (AudioClip ac in audios)
        {
            GameObject audio = Instantiate(audioPrefab);
            audio.GetComponent<Record>().SetAudio(ac);
            audio.transform.position = new Vector3(recordSpawn.position.x, height, recordSpawn.position.z);
            height += recordHeight;
            CollectionData.addToSounds(ac.name, audio);
        }

        gameObject.GetComponent<SaveLoad>().Load();
        gameObject.GetComponent<SaveLoadMedia>().Load();
    }
}
