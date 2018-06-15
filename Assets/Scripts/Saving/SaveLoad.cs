using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour {

    // Cheap way to indicate what user we want to load for now.
    public string userToLoad;

    public static SaveLoad saveFile;

    private Room saveRoom;
    private SaveTransform[] transToSave;
    private int toSaveCount;
    private int numToSave;

	public void Awake()
	{
        Load();
	}

    public void OnApplicationQuit()
    {
        // Set a SaveTransform for everything in the scene.
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        List<SaveTransform> toSave = new List<SaveTransform>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "GoldSpike") Debug.Log(obj.transform.position.y);
            SaveTransform st = new SaveTransform();
            st.objName = obj.name;
            st.xPosition = obj.transform.position.x;
            st.yPosition = obj.transform.position.y;
            st.zPosition = obj.transform.position.z;
            st.xScale = obj.transform.localScale.x;
            st.yScale = obj.transform.localScale.y;
            st.zScale = obj.transform.localScale.z;
            toSave.Add(st);
        }
        transToSave = toSave.ToArray();
        Save();
	}

	public void Save() {
        Room current = new Room();
        //Save the current room by saving the user and the transform of ST objects.
        current.user = userToLoad;
        current.objectTransforms = transToSave;

        // Save the updated room.
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedRoom" + userToLoad + ".rm");
        bf.Serialize(file, current);
        file.Close();
        Debug.Log("Saved data");
    }

    // Loads the correct room given user index.
    // Else, loads a default.
    // Use a negative index to indicate it is a new user.
    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedRoom" + userToLoad + ".rm"))
        {
            Debug.Log("Found save data");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedRoom" + userToLoad + ".rm", FileMode.Open);
            saveRoom = (Room)bf.Deserialize(file);
            file.Close();

            // Find data for the specific user, then load those objects in.
            // Otherwise, we'll start with the original, default scene.
            if (saveRoom != null)
            {
                Debug.Log("Loading save data.");

                // Set the saved transforms of objects.
                SaveTransform[] loadTransforms = saveRoom.objectTransforms;
                if (loadTransforms != null) {
                    foreach (SaveTransform st in loadTransforms)
                    {
                        GameObject toTransform = GameObject.Find(st.objName);

                        if (toTransform != null)
                        {
                            toTransform.transform.position = new Vector3(st.xPosition, st.yPosition, st.zPosition);
                            toTransform.transform.localScale = new Vector3(st.xScale, st.yScale, st.zScale);
                        }
                    }
                }
            }
        }
    }
}