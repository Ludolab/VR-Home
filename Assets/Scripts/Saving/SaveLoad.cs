using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour {

    // Cheap way to indicate what user we want to load for now.
    public string userToLoad;
    // Some bools for debugging purposes; whether or not to actively save/load.
    public bool saveNew;
    public bool loadPrevious;

    public static SaveLoad saveFile;

    private Room saveRoom;
    private Dictionary<int, ID> originalObjID = new Dictionary<int, ID>(); // Save reference to original objects (via instance IDs) and their IDs in the scene.
    private SaveObject[] objToSave;
    private SavePrefab[] prefabToSave;
    private ID[] objToDestroy;
    private int toSaveCount;
    private int numToSave;

    public void OnApplicationQuit()
    {
        if(saveNew) Save();
	}

	public void Save() {
        SaveObjects();
        Room current = new Room();
        //Save the current room by saving the user and the transform of ST objects.
        current.user = userToLoad;
        current.objects = objToSave;
        current.prefabs = prefabToSave;
        current.destroyObjects = objToDestroy;

        // Save the updated room.
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".rm");
        bf.Serialize(file, current);
        file.Close();
    }

    private void SaveObjects() {
        // Set a SaveObject or SavePrefab for everything in scene.
        GameObject[] allObjects = FindObjectsOfType<GameObject>(); // Get all game objects currently in the scene.
        List<SaveObject> saveObjects = new List<SaveObject>();
        List<SavePrefab> savePrefabs = new List<SavePrefab>();
        List<ID> saveDestroyed = new List<ID>();
        foreach (GameObject obj in allObjects)
        {
            if (UpdatedObjects.getCreatedObj().ContainsKey(obj.GetInstanceID()) && !UpdatedObjects.getDestroyedObj().Contains(obj.GetInstanceID()))
            { // Otherwise, add to loaded prefabs.

                SavePrefab sp = new SavePrefab();
                string val;
                UpdatedObjects.getCreatedObj().TryGetValue(obj.GetInstanceID(), out val);
                sp.prefabToLoad = val;
                sp.objData = new SaveObject();
                ID id = new ID();
                id.objName = obj.name;
                id.objCoordX = obj.transform.position.x;
                id.objCoordY = obj.transform.position.y;
                id.objCoordZ = obj.transform.position.z;
                sp.objData.objID = id;
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

                if (rend != null && rend.material.mainTexture != null && rend.material.mainTexture.name != "") sp.objData.texture = rend.material.mainTexture.name;
                AudioSource source = obj.GetComponent<AudioSource>();
                if (source != null && source.clip != null && source.clip.name != "") sp.objData.audio = source.clip.name;

                List<SaveObject> loadPrefabChildren = new List<SaveObject>();
                foreach (Transform child in obj.transform)
                {
                    SaveObject spc = new SaveObject();
                    ID childId = new ID();
                    childId.objName = child.name;
                    childId.objCoordX = child.position.x;
                    childId.objCoordY = child.position.y;
                    childId.objCoordZ = child.position.z;
                    spc.objID = childId;
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
                    if (childRend != null && childRend.material.mainTexture != null && rend.material.mainTexture.name != "") spc.texture = childRend.material.mainTexture.name;
                    AudioSource childSource = child.GetComponent<AudioSource>();
                    if (childSource != null && childSource.clip != null && source.clip.name != "") spc.audio = childSource.clip.name;

                    loadPrefabChildren.Add(spc);
                }

                   sp.childrenData = loadPrefabChildren.ToArray();
                   savePrefabs.Add(sp);

            } else if (UpdatedObjects.getDestroyedObj().Contains(obj.GetInstanceID())){
                ID id;
                originalObjID.TryGetValue(obj.GetInstanceID(), out id);
                if (id != null) saveDestroyed.Add(id);
            } else if (originalObjID.ContainsKey(obj.GetInstanceID()))
            { // Add to SaveObjects if originally in scene.

                SaveObject so = new SaveObject();
                ID id;
                originalObjID.TryGetValue(obj.GetInstanceID(), out id);
                so.objID = id;

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
                Renderer rend = obj.GetComponent<Renderer>();
                if (rend != null && rend.material.mainTexture != null && rend.material.mainTexture.name != "") so.texture = rend.material.mainTexture.name;
                AudioSource source = obj.GetComponent<AudioSource>();
                if (source != null && source.clip != null && source.clip.name != "") so.audio = source.clip.name;

                saveObjects.Add(so);
            }
        }
        objToSave = saveObjects.ToArray();
        prefabToSave = savePrefabs.ToArray();
        objToDestroy = saveDestroyed.ToArray();
    }

    // Loads the correct room given user index.
    // Else, loads a default.
    // Use a negative index to indicate it is a new user.
    public void Load()
    {
        // Save what objects were originally in the scene (i.e. not from a prefab/created during runtime).
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        List<int> originalObjects = allObjectInstanceID(allObjects);
        // Generate the IDs of objects.
        foreach (GameObject obj in allObjects)
        {
            ID id = new ID();
            id.objName = obj.name;
            id.objCoordX = obj.transform.position.x;
            id.objCoordY = obj.transform.position.y;
            id.objCoordZ = obj.transform.position.z;
            originalObjID.Add(obj.GetInstanceID(), id);
        }

        if (File.Exists(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".rm") && loadPrevious)
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

    private void LoadObjects(Room saveRoom)
    {
        // Set the saved transforms of objects.
        SaveObject[] loadObjState = saveRoom.objects;
        if (loadObjState != null)
        {
            foreach (SaveObject so in loadObjState)
            {
                GameObject toLoad = FindFromAll(so.objID);

                if (toLoad != null)
                {
                    toLoad.SetActive(so.objActive);
                    toLoad.transform.localPosition = new Vector3(so.xPosition, so.yPosition, so.zPosition);
                    toLoad.transform.localEulerAngles = new Vector3(so.xRotation, so.yRotation, so.zRotation);
                    toLoad.transform.localScale = new Vector3(so.xScale, so.yScale, so.zScale);
                    Renderer rend = toLoad.GetComponent<Renderer>();


                    if (toLoad.name == "Wall") Debug.Log(so.texture);


                    if (rend != null && so.texture != null && Resources.Load("Media/" + so.texture) != null) rend.material.mainTexture = (Texture2D)(Resources.Load("Media/" + so.texture));
                    AudioSource source = toLoad.GetComponent<AudioSource>();
                    if (source != null && so.audio != null && Resources.Load("Media/" + so.audio) != null) source.clip = (AudioClip)(Resources.Load("Media/" + so.audio));
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
                UpdatedObjects.addToCreated(loaded.GetInstanceID(), lp.prefabToLoad);
                loaded.SetActive(lp.objData.objActive);
                loaded.transform.localPosition = new Vector3(lp.objData.xPosition, lp.objData.yPosition, lp.objData.zPosition);
                loaded.transform.localEulerAngles = new Vector3(lp.objData.xRotation, lp.objData.yRotation, lp.objData.zRotation);
                loaded.transform.localScale = new Vector3(lp.objData.xScale, lp.objData.yScale, lp.objData.zScale);
                Renderer rend = loaded.GetComponent<Renderer>();
                if (rend != null && lp.objData.texture != null && Resources.Load("Media/" + lp.objData.texture) != null) rend.material.mainTexture = (Texture2D)(Resources.Load("Media/" + lp.objData.texture));
                AudioSource source = loaded.GetComponent<AudioSource>();
                if (source != null && lp.objData.audio != null && Resources.Load("Media/" + lp.objData.audio) != null) source.clip = (AudioClip)(Resources.Load("Media/" + lp.objData.audio));

                // Arrange the children of the prefab.
                foreach (SaveObject lpc in lp.childrenData)
                {
                    GameObject loadChild = loaded.transform.Find(lpc.objID.objName).gameObject;

                    if (loadChild != null)
                    {
                        loadChild.SetActive(lpc.objActive);
                        loadChild.transform.localPosition = new Vector3(lpc.xPosition, lpc.yPosition, lpc.zPosition);
                        loadChild.transform.localEulerAngles = new Vector3(lpc.xRotation, lpc.yRotation, lpc.zRotation);
                        loadChild.transform.localScale = new Vector3(lpc.xScale, lpc.yScale, lpc.zScale);
                        Renderer childRend = loadChild.GetComponent<Renderer>();
                        if (childRend != null && lpc.texture != null && Resources.Load("Media/" + lpc.texture)) childRend.material.mainTexture = (Texture2D)(Resources.Load("Media/" + lpc.texture));
                        AudioSource childSource = loadChild.GetComponent<AudioSource>();
                        if (childSource != null && lpc.audio != null && Resources.Load("Media/" + lpc.audio)) childSource.clip = (AudioClip)(Resources.Load("Media/" + lpc.audio));
                    }
                }
            }
        }


        // Destroy any gameobjects that the user deleted from the default.
        ID[] toDestroy = saveRoom.destroyObjects;
        if (toDestroy != null)
        {
            foreach (ID td in toDestroy){
                GameObject destroy = FindFromAll(td);
                if (destroy != null) Destroy(destroy);
            }
        }
    }

    private List<int> allObjectInstanceID(GameObject[] objects)
    {
        List<int> objIDs = new List<int> ();
        foreach (GameObject obj in objects)
        {
            objIDs.Add(obj.GetInstanceID());
        }
        return objIDs;
    }

    private GameObject FindFromAll(ID toFind) {

        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects) {

            if (obj.name == toFind.objName
                && (int)(obj.transform.position.x * 100.0) == (int)(toFind.objCoordX * 100.0)
                && (int)(obj.transform.position.y * 100.0) == (int)(toFind.objCoordY * 100.0)
                && (int)(obj.transform.position.z * 100.0) == (int)(toFind.objCoordZ * 100.0)) {
                return obj;
            }
        }
        return null;
    }
}