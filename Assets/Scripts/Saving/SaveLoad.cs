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
    private SavePrefab[] prefabToSave;
    private int toSaveCount;
    private int numToSave;

	public void Start()
	{
        // Save what objects were originally in the scene (i.e. not from a prefab/created during runtime).
        originalObjects = allObjectID();

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
        FileStream file = File.Create(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".rm");
        bf.Serialize(file, current);
        file.Close();
    }

    private void SaveObjects() {
        // Set a SaveObject or SavePrefab for everything in scene.
        List<GameObject> allObj = allObjects(); // Get all game objects currently in the scene.
        List<SaveObject> saveObjects = new List<SaveObject>();
        List<SavePrefab> savePrefabs = new List<SavePrefab>();
        foreach (GameObject obj in allObj)
        {
            
            if (CreatedPrefabs.getCreatedObj().ContainsKey(obj.GetInstanceID()))
            { // Otherwise, add to loaded prefabs.

                SavePrefab sp = new SavePrefab();
                string val;
                CreatedPrefabs.getCreatedObj().TryGetValue(obj.GetInstanceID(), out val);
                sp.prefabToLoad = val;
                sp.objData = new SaveObject();
                sp.objData.objActive = obj.activeSelf;
                sp.objData.xPosition = obj.transform.localPosition.x;
                sp.objData.yPosition = obj.transform.localPosition.y;
                sp.objData.zPosition = obj.transform.localPosition.z;
                sp.objData.xRotation = obj.transform.localEulerAngles.x;
                sp.objData.yRotation = obj.transform.localEulerAngles.y;
                sp.objData.zRotation = obj.transform.localEulerAngles.z;
                sp.objData.xScale = obj.transform.localScale.x;
                sp.objData.yScale = obj.transform.localScale.y;
                sp.objData.zScale = obj.transform.localScale.z;
                Renderer rend = obj.GetComponent<Renderer>();
                if (rend != null && rend.material.mainTexture != null) sp.objData.texture = rend.material.mainTexture.name;

                List<SaveObject> loadPrefabChildren = new List<SaveObject>();
                foreach (Transform child in obj.transform)
                {
                    SaveObject spc = new SaveObject();
                    spc.objName = child.name;
                    spc.objActive = child.gameObject.activeSelf;
                    spc.xPosition = child.localPosition.x;
                    spc.yPosition = child.localPosition.y;
                    spc.zPosition = child.localPosition.z;
                    spc.xRotation = child.localEulerAngles.x;
                    spc.yRotation = child.localEulerAngles.y;
                    spc.zRotation = child.localEulerAngles.z;
                    spc.xScale = child.localScale.x;
                    spc.yScale = child.localScale.y;
                    spc.zScale = child.localScale.z;
                    Renderer childRend = child.GetComponent<Renderer>();
                    if (childRend != null && childRend.material.mainTexture != null) spc.texture = childRend.material.mainTexture.name;
                    loadPrefabChildren.Add(spc);
                }
                   sp.childrenData = loadPrefabChildren.ToArray();
                   savePrefabs.Add(sp);
            } else if (originalObjects.Contains(obj.GetInstanceID()))
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
                Renderer rend = obj.GetComponent<Renderer>();
                if (rend != null && rend.material.mainTexture != null) st.texture = rend.material.mainTexture.name;

                saveObjects.Add(st);
            }
        }
        objToSave = saveObjects.ToArray();
        prefabToSave = savePrefabs.ToArray();

    }

    // Loads the correct room given user index.
    // Else, loads a default.
    // Use a negative index to indicate it is a new user.
    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".rm"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".rm", FileMode.Open);
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
                    ImageQuad imgQuad = toLoad.GetComponent<ImageQuad>();
                    if (imgQuad != null && Resources.Load("Photos/" + st.texture) != null) imgQuad.SetTexture((Texture2D)(Resources.Load("Photos/" + st.texture)));
                }
            }
        }

        // Load in any prefab objects created previously.
        SavePrefab[] loadPrefabState = saveRoom.prefabs;
        if (loadPrefabState != null)
        {
            foreach (SavePrefab lp in loadPrefabState)
            {
                GameObject loaded = (GameObject)Instantiate(Resources.Load(lp.prefabToLoad));
                CreatedPrefabs.addToCreated(loaded.GetInstanceID(), lp.prefabToLoad);
                loaded.SetActive(lp.objData.objActive);
                loaded.transform.localPosition = new Vector3(lp.objData.xPosition, lp.objData.yPosition, lp.objData.zPosition);
                loaded.transform.localEulerAngles = new Vector3(lp.objData.xRotation, lp.objData.yRotation, lp.objData.zRotation);
                loaded.transform.localScale = new Vector3(lp.objData.xScale, lp.objData.yScale, lp.objData.zScale);
                ImageQuad imgQuad = loaded.GetComponent<ImageQuad>();
                if(imgQuad != null && Resources.Load("Photos/" + lp.objData.texture) != null) imgQuad.SetTexture((Texture2D)(Resources.Load("Photos/" + lp.objData.texture)));

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
                        Renderer rend = loadChild.GetComponent<Renderer>();
                        if(rend != null)rend.material.mainTexture = (Texture2D)(Resources.Load("Photos/" + lpc.texture));
                    }
                }
            }
        }
    }

    public List<GameObject> allObjects()
    {
        GameObject[] allParents = FindObjectsOfType<GameObject>();
        List<GameObject> allObj = new List<GameObject>();
        foreach (GameObject obj in allParents)
        {
            allObj.Add(obj);
            allObj.AddRange(getChildren(obj.transform, new List<GameObject>()));
        }

        return allObj;
    }

    public List<GameObject> getChildren(Transform parent, List<GameObject> list)
    {
        foreach (Transform child in parent)
        {
            if (child == null) continue; //No more children to recurse on so break.

            list.Add(child.gameObject);
            getChildren(child, list);
        }
        return list;
    }

    public List<int> allObjectID()
    {
        List<GameObject> all = allObjects();
        List<int> allID = new List<int>();
        foreach (GameObject obj in all)
        {
            allID.Add(obj.GetInstanceID());
        }
        return allID;
    }
}