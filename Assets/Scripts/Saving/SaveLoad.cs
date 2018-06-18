using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SaveLoad : MonoBehaviour {

    // Cheap way to indicate what user we want to load for now.
    public string userToLoad;

    public static SaveLoad saveFile;

    private Room saveRoom;
    private SaveObject[] objToSave;
    private List<int> originalObjects = new List<int>(); // Save reference to original objects, not from prefabs, in the scene.
    private Dictionary<int, string> prefabObjects = new Dictionary<int, string>(); // Save reference to objects created from prefabs in loading.
    private LoadPrefab[] prefabToSave;
    private int toSaveCount;
    private int numToSave;

	public void Start()
	{
        // Save what objects were originally in the scene (i.e. not from a prefab/created during runtime).
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            originalObjects.Add(obj.GetInstanceID());
        }

        Load();
	}

    public void OnApplicationQuit()
    {
        Save();
	}

	public void Save() {
        SaveObjects();
        Room current = new Room();
        //Save the current room by saving the user and the transform of ST objects.
        current.user = userToLoad;
        current.objects = objToSave;
        current.prefabs = prefabToSave;

        // Save the updated room.
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedRoom" + userToLoad + ".rm");
        bf.Serialize(file, current);
        file.Close();
    }

    private void SaveObjects() {
        // Set a SaveObject or LoadPrefab for everything in scene.
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        List<SaveObject> saveObjects = new List<SaveObject>();
        List<LoadPrefab> loadPrefabs = new List<LoadPrefab>();
        foreach (GameObject obj in allObjects)
        {
            if (originalObjects.Contains(obj.GetInstanceID()))
            { // Add to SaveObjects if originally in scene.
                SaveObject st = new SaveObject();
                st.objName = obj.name;
                st.objActive = obj.activeSelf;
                st.xPosition = obj.transform.localPosition.x;
                st.yPosition = obj.transform.localPosition.y;
                st.zPosition = obj.transform.localPosition.z;
                st.xRotation = obj.transform.localEulerAngles.x;
                st.yRotation = obj.transform.localEulerAngles.y;
                st.zRotation = obj.transform.localEulerAngles.z;
                st.xScale = obj.transform.localScale.x;
                st.yScale = obj.transform.localScale.y;
                st.zScale = obj.transform.localScale.z;

                saveObjects.Add(st);
            }
            else if (PrefabUtility.GetPrefabParent(obj) != null || prefabObjects.ContainsKey(obj.GetInstanceID()))
            { // Otherwise, add to loaded prefabs.
                Debug.Log("Found prefab");
                LoadPrefab lp = new LoadPrefab();
                if (PrefabUtility.GetPrefabParent(obj) != null){
                    lp.prefabToLoad = PrefabUtility.GetPrefabParent(obj).name;
                } else {
                    string val;
                    prefabObjects.TryGetValue(obj.GetInstanceID(), out val);
                    lp.prefabToLoad = val;
                }
                lp.objData = new SaveObject();
                lp.objData.objActive = obj.activeSelf;
                lp.objData.xPosition = obj.transform.localPosition.x;
                lp.objData.yPosition = obj.transform.localPosition.y;
                lp.objData.zPosition = obj.transform.localPosition.z;
                lp.objData.xRotation = obj.transform.localEulerAngles.x;
                lp.objData.yRotation = obj.transform.localEulerAngles.y;
                lp.objData.zRotation = obj.transform.localEulerAngles.z;
                lp.objData.xScale = obj.transform.localScale.x;
                lp.objData.yScale = obj.transform.localScale.y;
                lp.objData.zScale = obj.transform.localScale.z;

                List<SaveObject> loadPrefabChildren = new List<SaveObject>();
                foreach (Transform child in obj.transform)
                {
                    SaveObject lpc = new SaveObject();
                    lpc.objName = child.name;
                    lpc.objActive = child.gameObject.activeSelf;
                    lpc.xPosition = child.localPosition.x;
                    lpc.yPosition = child.localPosition.y;
                    lpc.zPosition = child.localPosition.z;
                    lpc.xRotation = child.localEulerAngles.x;
                    lpc.yRotation = child.localEulerAngles.y;
                    lpc.zRotation = child.localEulerAngles.z;
                    lpc.xScale = child.localScale.x;
                    lpc.yScale = child.localScale.y;
                    lpc.zScale = child.localScale.z;
                    loadPrefabChildren.Add(lpc);
                }
                lp.childrenData = loadPrefabChildren.ToArray();

                loadPrefabs.Add(lp);
            }
        }
        objToSave = saveObjects.ToArray();
        prefabToSave = loadPrefabs.ToArray();

    }

    // Loads the correct room given user index.
    // Else, loads a default.
    // Use a negative index to indicate it is a new user.
    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedRoom" + userToLoad + ".rm"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedRoom" + userToLoad + ".rm", FileMode.Open);
            saveRoom = (Room)bf.Deserialize(file);
            file.Close();

            // Find data for the specific user, then load those objects in.
            // Otherwise, we'll start with the original, default scene.
            if (saveRoom != null)
            {
                LoadObjects(saveRoom);
            }
        }
    }

    public void LoadObjects(Room saveRoom) {
        // Set the saved transforms of objects.
        SaveObject[] loadObjState = saveRoom.objects;
        if (loadObjState != null)
        {
            foreach (SaveObject st in loadObjState)
            {
                GameObject toLoad = GameObject.Find(st.objName);

                if (toLoad != null)
                {
                    toLoad.SetActive(st.objActive);
                    toLoad.transform.localPosition = new Vector3(st.xPosition, st.yPosition, st.zPosition);
                    toLoad.transform.localEulerAngles = new Vector3(st.xRotation, st.yRotation, st.zRotation);
                    toLoad.transform.localScale = new Vector3(st.xScale, st.yScale, st.zScale);
                }
            }
        }

        // Load in any prefab objects created previously.
        LoadPrefab[] loadPrefabState = saveRoom.prefabs;
        if (loadPrefabState != null)
        {
            foreach (LoadPrefab lp in loadPrefabState)
            {
                GameObject loaded = (GameObject)Instantiate(Resources.Load(lp.prefabToLoad));
                prefabObjects.Add(loaded.GetInstanceID(),lp.prefabToLoad);
                loaded.SetActive(lp.objData.objActive);
                loaded.transform.localPosition = new Vector3(lp.objData.xPosition, lp.objData.yPosition, lp.objData.zPosition);
                loaded.transform.localEulerAngles = new Vector3(lp.objData.xRotation, lp.objData.yRotation, lp.objData.zRotation);
                loaded.transform.localScale = new Vector3(lp.objData.xScale, lp.objData.yScale, lp.objData.zScale);

                // Arrange the children of the prefab.
                foreach (SaveObject lpc in lp.childrenData)
                {
                    GameObject loadChild = loaded.transform.Find(lpc.objName).gameObject;

                    if (loadChild != null)
                    {
                        loadChild.SetActive(lpc.objActive);
                        loadChild.transform.localPosition = new Vector3(lpc.xPosition, lpc.yPosition, lpc.zPosition);
                        loadChild.transform.localEulerAngles = new Vector3(lpc.xRotation, lpc.yRotation, lpc.zRotation);
                        loadChild.transform.localScale = new Vector3(lpc.xScale, lpc.yScale, lpc.zScale);
                    }
                }
            }
        }
    }

}