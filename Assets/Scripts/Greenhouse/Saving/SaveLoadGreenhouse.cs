using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SaveLoadGreenhouse : MonoBehaviour {

    public string userToLoad;
    public bool saveNew;
    public bool loadPrevious;
    public string pathToPlantPrefabs;

	private void Start () {
        if (loadPrevious) Load();
	}

	private void OnApplicationQuit()
	{
        if (saveNew) Save();
	}

	private void Save() {
        SavePlot[] plots = SavePlots();
        //TODO: save state of in/outboxes.
        GreenhouseSave current = new GreenhouseSave();
        current.previousDay = TimeManager.instance.GetDay();
        current.plots = plots;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".gh");
        bf.Serialize(file, current);
        file.Close();
    }

    private SavePlot[] SavePlots() {
        Plot[] toSave = TimeManager.instance.plots;
        SavePlot[] plots = new SavePlot[toSave.Length];
        for (int i = 0; i < toSave.Length; i++) {
            Plot curr = toSave[i];
            SavePlot plotSave = new SavePlot();

            ID plotID = new ID();
            plotID.objCoordX = curr.transform.position.x;
            plotID.objCoordZ = curr.transform.position.z;
            plotSave.plotID = plotID;

            //Make sure we purely get the name of the prefab of the plant.
            string plantName = curr.getPlant().name;
            int truncateIndex = plantName.IndexOf(" ");
            if(truncateIndex > 0) {
                plantName = plantName.Substring(0, truncateIndex);
            }
            plotSave.plant = plantName;

            plotSave.plantDayBorn = curr.getPlant().getDayBorn();
            plotSave.plantStage = curr.getPlant().getStage();
            //TODO: save watering.

            List<SaveObject> weeds = new List<SaveObject>();
            foreach(GameObject weed in curr.getWeeds()) {
                SaveObject saveWeed = new SaveObject();
                saveWeed.xPosition = weed.transform.position.x;
                saveWeed.yPosition = weed.transform.position.y;
                saveWeed.zPosition = weed.transform.position.z;
                saveWeed.xRotation = weed.transform.eulerAngles.x;
                saveWeed.yRotation = weed.transform.eulerAngles.y;
                saveWeed.zRotation = weed.transform.eulerAngles.z;
                saveWeed.xScale = weed.transform.localScale.x;
                saveWeed.yScale = weed.transform.localScale.y;
                saveWeed.zScale = weed.transform.localScale.z;

                weeds.Add(saveWeed);
            }
            plotSave.weeds = weeds.ToArray();

            plotSave.beetles = curr.getBeetleIDs();
            plotSave.fruits = curr.getFruitIDs();
        }

        return plots;
    }

    private void Load() {
        if (File.Exists(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".gh"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".gh", FileMode.Open);
            GreenhouseSave saved = (GreenhouseSave)bf.Deserialize(file);
            file.Close();

            // Find data for the specific user, then load those objects in.
            // Otherwise, we'll start with the original, default scene.
            if (saved != null)
            {
                TimeManager.instance.SetDay(saved.previousDay);
                LoadPlots(saved.plots);
                //TODO: load in state of in/outboxes from the previous session for processing.
                //TODO: save where picked but unsent fruits are?

                //After restoring the state of the previous play session, advance to the next day.
                TimeManager.instance.NextDay();
            }
        }
    }

    private void LoadPlots(SavePlot[] savedPlots) {
        Plot[] plots = TimeManager.instance.plots;
        foreach (Plot plot in plots)
        {
            SavePlot savedData = FindSavedPlot(plot, savedPlots);

            //Load in data for plant in plot.
            if(savedData.plant != "") {
                GameObject loadPlant = (GameObject)Instantiate(Resources.Load(pathToPlantPrefabs + savedData.plant));
                Plant plant = loadPlant.GetComponent<Plant>();
                if(plant != null) {
                    plot.setPlant(plant);
                    plant.setDayBorn(savedData.plantDayBorn);
                    plant.setStage(savedData.plantStage);

                    //Load in beetles and fruit leftover from previous session.
                    Transform[] beetles = null;
                    if (savedData.plantStage == 1)
                    {
                        beetles = plant.beetleTransYoung;
                    } else if (savedData.plantStage >= 2) {
                        beetles = plant.beetleTransGrown;
                    }
                    if (beetles != null && savedData.beetles != null)
                    {
                        foreach (int beetleID in savedData.beetles)
                        {
                            GameObject beetle = Instantiate(plot.beetlePrefab);
                            beetle.transform.position = plant.transform.position + plant.beetleTransGrown[beetleID].localPosition;
                            beetle.transform.eulerAngles = plant.transform.eulerAngles + plant.beetleTransGrown[beetleID].localEulerAngles;
                            beetle.transform.localScale = plant.beetleTransGrown[beetleID].localScale;

                            plot.addToBeetles(beetle, beetleID);
                        }
                    }
                    if(savedData.plantStage >= 2 && savedData.fruits != null)
                    {
                        foreach (int fruitID in savedData.fruits)
                        {
                            GameObject fruit = Instantiate(plant.fruit);
                            fruit.transform.position = plant.transform.position + plant.fruitTrans[fruitID].localPosition;
                            fruit.transform.eulerAngles = plant.transform.eulerAngles + plant.fruitTrans[fruitID].localEulerAngles;
                            fruit.transform.localScale = plant.fruitTrans[fruitID].localScale;

                            plot.addToFruit(fruit, fruitID);
                        }
                    }
                }
            }

            //Set weeds left in plot.
            if(savedData.weeds != null) {
                foreach(SaveObject savedWeed in savedData.weeds) {
                    GameObject weed = Instantiate(plot.weedPrefab);
                    weed.transform.position = new Vector3(savedWeed.xPosition, savedWeed.yPosition, savedWeed.zPosition);
                    weed.transform.eulerAngles = new Vector3(savedWeed.xRotation, savedWeed.yRotation, savedWeed.zRotation);
                    weed.transform.localScale = new Vector3(savedWeed.xScale, savedWeed.yScale, savedWeed.zScale);

                    plot.addToWeeds(weed);
                }
            }

            //TODO: set if previously watered or not.

        }
    }

    private SavePlot FindSavedPlot(Plot toFind, SavePlot[] savedPlots) {
        foreach(SavePlot saved in savedPlots) {
            if((int)(toFind.transform.position.x * 100) == (int)(saved.plotID.objCoordX * 100)
               && (int)(toFind.transform.position.z * 100) == (int)(saved.plotID.objCoordZ * 100)) {
                return saved;
            }
        }
        return null;
    }
}
