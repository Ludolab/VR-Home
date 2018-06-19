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
    private List<int> originalObjects = new List<int>(); // Save reference to original objects, not from prefabs, in the scene.
    private Dictionary<int, ID> originalObjID = new Dictionary<int, ID>();
    private SaveObject[] objToSave;
    private SavePrefab[] prefabToSave;
    private ID[] objToDestroy;
    private int toSaveCount;
    private int numToSave;

	public void Start()
	{
        // Save what objects were originally in the scene (i.e. not from a prefab/created during runtime).
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        originalObjects = allObjectID(allObjects);
        // Generate the IDs of objects.
        foreach (GameObject obj in allObjects) {
            ID id = new ID();
            id.objName = obj.name;
            id.objCoordX = obj.transform.position.x;
            id.objCoordY = obj.transform.position.y;
            id.objCoordZ = obj.transform.position.z;
            originalObjID.Add(obj.GetInstanceID(), id);
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
                if (rend != null && rend.material.mainTexture != null) sp.objData.texture = rend.material.mainTexture.name;

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
                    if (childRend != null && childRend.material.mainTexture != null) spc.texture = childRend.material.mainTexture.name;
                    loadPrefabChildren.Add(spc);
                }
                   sp.childrenData = loadPrefabChildren.ToArray();
                   savePrefabs.Add(sp);
            } else if (UpdatedObjects.getDestroyedObj().Contains(obj.GetInstanceID())){
                ID id;
                originalObjID.TryGetValue(obj.GetInstanceID(), out id);
                if (id != null) saveDestroyed.Add(id);
            } else if (originalObjects.Contains(obj.GetInstanceID()))
            { // Add to SaveObjects if originally in scene.

                SaveObject st = new SaveObject();
                ID id;
                originalObjID.TryGetValue(obj.GetInstanceID(), out id);
                st.objID = id;

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
        objToDestroy = saveDestroyed.ToArray();
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

    public void LoadObjects(Room saveRoom)
    {
        // Set the saved transforms of objects.
        SaveObject[] loadObjState = saveRoom.objects;
        if (loadObjState != null)
        {
            foreach (SaveObject st in loadObjState)
            {
                GameObject toLoad = FindFromAll(st.objID);

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
                UpdatedObjects.addToCreated(loaded.GetInstanceID(), lp.prefabToLoad);
                loaded.SetActive(lp.objData.objActive);
                loaded.transform.localPosition = new Vector3(lp.objData.xPosition, lp.objData.yPosition, lp.objData.zPosition);
                loaded.transform.localEulerAngles = new Vector3(lp.objData.xRotation, lp.objData.yRotation, lp.objData.zRotation);
                loaded.transform.localScale = new Vector3(lp.objData.xScale, lp.objData.yScale, lp.objData.zScale);
                ImageQuad imgQuad = loaded.GetComponent<ImageQuad>();
                if (imgQuad != null && Resources.Load("Photos/" + lp.objData.texture) != null) imgQuad.SetTexture((Texture2D)(Resources.Load("Photos/" + lp.objData.texture)));

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
                        Renderer rend = loadChild.GetComponent<Renderer>();
                        if (rend != null) rend.material.mainTexture = (Texture2D)(Resources.Load("Photos/" + lpc.texture));
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

    public List<int> allObjectID(GameObject[] objects)
    {
        List<int> objIDs = new List<int> ();
        foreach (GameObject obj in objects)
        {
            objIDs.Add(obj.GetInstanceID());
        }
        return objIDs;
    }

    public GameObject FindFromAll(ID toFind) {

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