using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveLoadMedia : MonoBehaviour {

    private MediaData[] mediaSave;
    private MediaData[] mediaLoad;
    private string userToLoad;

	private void Start()
	{
        userToLoad = gameObject.GetComponent<SaveLoad>().userToLoad;
	}

	public void Save () {
        SaveImages();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".images");
        bf.Serialize(file, mediaSave);
        file.Close();
	}

    public void SaveImages() {
        List<GameObject> images = CollectionData.getImages();
        List<GameObject> sounds = CollectionData.getSounds();
        List<MediaData> toSave = new List<MediaData>();
        foreach (GameObject img in images) {
            MediaData save = new MediaData();
            save.imgActive = img.activeSelf;
            save.xPosition = img.transform.localPosition.x;
            save.yPosition = img.transform.localPosition.y;
            save.zPosition = img.transform.localPosition.z;
            save.xRotation = img.transform.localEulerAngles.x;
            save.yRotation = img.transform.localEulerAngles.y;
            save.zRotation = img.transform.localEulerAngles.z;
            save.xScale = img.transform.localScale.x;
            save.yScale = img.transform.localScale.y;
            save.zScale = img.transform.localScale.z;
            Renderer rend = img.GetComponent<Renderer>();
            if (rend != null && rend.material.mainTexture != null) save.media = rend.material.mainTexture.name;

            toSave.Add(save);
        }
        foreach (GameObject sound in sounds)
        {
            MediaData save = new MediaData();
            save.imgActive = sound.activeSelf;
            save.xPosition = sound.transform.localPosition.x;
            save.yPosition = sound.transform.localPosition.y;
            save.zPosition = sound.transform.localPosition.z;
            save.xRotation = sound.transform.localEulerAngles.x;
            save.yRotation = sound.transform.localEulerAngles.y;
            save.zRotation = sound.transform.localEulerAngles.z;
            save.xScale = sound.transform.localScale.x;
            save.yScale = sound.transform.localScale.y;
            save.zScale = sound.transform.localScale.z;
            AudioSource aud = sound.GetComponent<AudioSource>();
            if (aud != null && aud.clip != null) save.media = aud.clip.name;

            toSave.Add(save);
        }
        mediaSave = toSave.ToArray();
    }
	
	public void Load () {
        if (File.Exists(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".images"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".images", FileMode.Open);
            mediaLoad = (MediaData[])bf.Deserialize(file);
            file.Close();

            // Load in data for images from a previous playthrough.
            if (mediaLoad != null)
            {
                LoadImages(mediaLoad);
            }
        }
	}

    public void LoadImages(MediaData[] medias) {
        foreach (MediaData media in medias) {
            GameObject load = FindImgOrAudio(media);

            if(load != null) {
                load.SetActive(media.imgActive);
                load.transform.localPosition = new Vector3(media.xPosition, media.yPosition, media.zPosition);
                load.transform.localEulerAngles = new Vector3(media.xRotation, media.yRotation, media.zRotation);
                load.transform.localScale = new Vector3(media.xScale, media.yScale, media.zScale);
            }
        }
    }

    public GameObject FindImgOrAudio(MediaData media) {
        List<GameObject> imgObjects = CollectionData.getImages();
        List<GameObject> soundObjects = CollectionData.getSounds();
        foreach (GameObject obj in imgObjects)
        {
            Renderer rend = obj.GetComponent<Renderer>();
            if (rend != null
                && rend.material.mainTexture != null
                && rend.material.mainTexture.name == media.media)
            {
                return obj;
            }
        }
        foreach (GameObject obj in soundObjects)
        {
            AudioSource aud = obj.GetComponent<AudioSource>();
            if(aud != null
               && aud.clip != null
               && aud.clip.name == media.media)
            {
                return obj;
            }
        }
        return null;
    }
}
