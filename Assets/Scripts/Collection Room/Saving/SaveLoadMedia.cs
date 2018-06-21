using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SaveLoadMedia : MonoBehaviour {

    private SaveObject[] imgToSave;
    private SaveObject[] vidToSave;
    private SaveObject[] soundToSave;

    private string userToLoad;
    private bool saveNew;
    private bool loadPrevious;

    private GameObject test;

	private void Start()
	{
        userToLoad = gameObject.GetComponent<SaveLoad>().userToLoad;
        saveNew = gameObject.GetComponent<SaveLoad>().saveNew;
        loadPrevious = gameObject.GetComponent<SaveLoad>().loadPrevious;
	}

	private void OnApplicationQuit()
	{
        if(saveNew) Save();
	}

	private void Save() {
        SaveMedia();
        SaveMedia current = new SaveMedia();
        current.images = imgToSave;
        current.videos = vidToSave;
        current.sounds = soundToSave;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".media");
        bf.Serialize(file, current);
        file.Close();
    }

    private void SaveMedia() {
        List<SaveObject> saveImages = new List<SaveObject>();
        List<SaveObject> saveVideos = new List<SaveObject>();
        List<SaveObject> saveSounds = new List<SaveObject>();

        foreach(KeyValuePair<string, GameObject> img in CollectionData.getImages())
        {
            AddMediaToList(img, saveImages);
        }
        foreach (KeyValuePair<string, GameObject> vid in CollectionData.getVideos())
        {
            AddMediaToList(vid, saveVideos);
        }
        foreach (KeyValuePair<string, GameObject> sound in CollectionData.getSounds())
        {
            AddMediaToList(sound, saveSounds);
        }
        imgToSave = saveImages.ToArray();
        vidToSave = saveVideos.ToArray();
        soundToSave = saveSounds.ToArray();
    }

    private void AddMediaToList(KeyValuePair<string, GameObject> media, List<SaveObject> list) {
        SaveObject so = new SaveObject();
        GameObject obj = media.Value;

        so.objActive = obj.activeSelf;
        so.xPosition = obj.transform.localPosition.x;
        so.yPosition = obj.transform.localPosition.y;
        so.zPosition = obj.transform.localPosition.z;
        so.xRotation = obj.transform.localEulerAngles.x;
        so.yRotation = obj.transform.localEulerAngles.y;
        so.zRotation = obj.transform.localEulerAngles.z;
        so.xScale = obj.transform.localScale.x;
        so.yScale = obj.transform.localScale.y;
        so.zScale = obj.transform.localScale.z;

        if (obj.transform.childCount > 0)
        {
            Transform child = obj.transform.GetChild(0);
            Renderer quad = child.gameObject.GetComponent<Renderer>();
            if (quad != null && quad.material.mainTexture != null && quad.material.mainTexture.name != "") so.texture = quad.material.mainTexture.name;
            VideoPlayer player = child.gameObject.GetComponent<VideoPlayer>();
            if (player != null && player.clip != null && player.clip.name != "") so.video = player.clip.name;
        }

        Record sound = obj.GetComponent<Record>();
        if (sound != null && sound.GetAudio() != null && sound.GetAudio().name != "") so.audio = sound.GetAudio().name;

        list.Add(so);
    }

    public void Load() {
        if (File.Exists(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".media") && loadPrevious)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".media", FileMode.Open);
            SaveMedia media = (SaveMedia)bf.Deserialize(file);
            file.Close();

            // Find data for the specific user, then load those objects in.
            // Otherwise, we'll start with the original, default scene.
            if (media != null)
            {
                LoadMedia(media);
            }
        }
    }

    private void LoadMedia(SaveMedia media) {
        SaveObject[] images = media.images;
        SaveObject[] videos = media.videos;
        SaveObject[] sounds = media.sounds;

        Dictionary<string, GameObject> imgLoaded = CollectionData.getImages();
        Dictionary<string, GameObject> vidLoaded = CollectionData.getVideos();
        Dictionary<string, GameObject> soundLoaded = CollectionData.getSounds();

        foreach(SaveObject img in images) {
            GameObject obj;
            imgLoaded.TryGetValue(img.texture, out obj);

            if(obj != null) {
                SetObject(obj, img);
            }
        }

        foreach (SaveObject vid in videos)
        {
            GameObject obj;
            vidLoaded.TryGetValue(vid.video, out obj);

            if (obj != null)
            {
                SetObject(obj, vid);
            }
        }

        foreach (SaveObject sound in sounds)
        {
            GameObject obj;
            soundLoaded.TryGetValue(sound.audio, out obj);

            if (obj != null)
            {
                SetObject(obj, sound);
            }
        }
    }

    private void SetObject(GameObject obj, SaveObject reference) {
        obj.SetActive(reference.objActive);
        obj.transform.localPosition = new Vector3(reference.xPosition, reference.yPosition, reference.zPosition);
        obj.transform.localEulerAngles = new Vector3(reference.xRotation, reference.yRotation, reference.zRotation);
        obj.transform.localScale = new Vector3(reference.xScale, reference.yScale, reference.zScale);
    }
}
