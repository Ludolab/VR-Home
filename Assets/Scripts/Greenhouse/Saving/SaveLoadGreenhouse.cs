using Leap.Unity.Interaction;
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
        GreenhouseSave current = new GreenhouseSave();
        Debug.Log("Now saving Day " + TimeManager.instance.GetDay());
        current.previousDay = TimeManager.instance.GetDay();
        current.plots = SavePlots();
        current.outboxes = SaveOutboxes();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedRoom" + SceneManager.GetActiveScene().name + userToLoad + ".gh");
        bf.Serialize(file, current);
        file.Close();
    }

    // Save data of plots (plant in it, remaining weeds/beetles/fruits, watered amount, etc.)
    private SavePlot[] SavePlots() {
        Plot[] toSave = TimeManager.instance.plots;
        SavePlot[] plots = new SavePlot[toSave.Length];
        for (int i = 0; i < toSave.Length; i++)
        {
            Plot curr = toSave[i];
            Debug.Log("Processing plot " + curr.gameObject.name);
            SavePlot plotSave = new SavePlot();

            ID plotID = new ID();
            plotID.objCoordX = curr.transform.position.x;
            plotID.objCoordZ = curr.transform.position.z;
            plotSave.plotID = plotID;

            if(curr.getPlant() != null) {
                Debug.Log("Found plant " + curr.getPlant().plant + ". Now saving its data.");
                plotSave.plant = curr.getPlant().plant;
                plotSave.plantDayBorn = curr.getPlant().getDayBorn();
                plotSave.plantStage = curr.getPlant().getStage();
                plotSave.watered = curr.myDirt.getWetness();
            }

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

            plots[i] = plotSave;
        }

        return plots;
    }

    // Save outboxes, including gifts the player is sending, remaining stuff from the neighbor, etc.
    private SaveOutbox[] SaveOutboxes() {
        Outbox[] toSave = TimeManager.instance.outboxes;
        SaveOutbox[] outboxes = new SaveOutbox[toSave.Length];
        for (int i = 0; i < toSave.Length; i++) {
            Outbox curr = toSave[i];
            SaveOutbox outboxSave = new SaveOutbox();

            outboxSave.neighbor = curr.GetLabel();
            outboxSave.givenGifts = curr.GetGiftNames();

            outboxes[i] = outboxSave;
        }

        return outboxes;
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
                Debug.Log("Found saved data. Now loading Day " + saved.previousDay);
                TimeManager.instance.SetDay(saved.previousDay);
                LoadPlots(saved.plots);
                LoadOutboxes(saved.outboxes);
            }
        }

        //After restoring the state of the previous play session, advance to the next day.
        TimeManager.instance.NextDay();
    }

    private void LoadPlots(SavePlot[] savedPlots) {
        Plot[] plots = TimeManager.instance.plots;
        foreach (Plot plot in plots)
        {
            Debug.Log("Processing plot " + plot.gameObject.name);
            SavePlot savedData = FindSavedPlot(plot, savedPlots);

            //Load in data for plant in plot.
            if(savedData.plant != null && savedData.plant != "") {
                GameObject loadPlant = (GameObject)Instantiate(Resources.Load(pathToPlantPrefabs + savedData.plant));
                Plant plant = loadPlant.GetComponent<Plant>();
                if(plant != null) {
                    Debug.Log("Found a plant of type " + savedData.plant + "! Now restoring previous day data.");
                    Debug.Log("Plant is of stage " + savedData.plantStage + " previously");
                    plot.setPlant(plant, savedData.plantStage);
                    plant.setDayBorn(savedData.plantDayBorn);

                    //Load in beetles leftover from previous session.
                    if(savedData.beetles != null) {
                        if (savedData.plantStage == 1)
                        {
                            SetBeetles(plot, savedData.beetles, plant.beetleTrans1);
                        }
                        else if (savedData.plantStage >= 2)
                        {
                            SetBeetles(plot, savedData.beetles, plant.beetleTrans2);
                        }
                    }

                    //Load in unpicked fruits leftover from previous session.
                    if(savedData.plantStage == plant.nonFruitingStages && savedData.fruits != null)
                    {
                        SetFruits(plot, savedData.fruits, plant.fruitTrans);
                    }
                }
            }

            //Set weeds left in plot.
            if(savedData.weeds != null) {
                SetWeeds(plot, savedData.weeds);
            }

            //Set how watered the plot was.
            plot.myDirt.setWetness(savedData.watered);

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

    private void SetBeetles(Plot plot, int[] beetleIDs, Transform[] transforms) {
        foreach (int beetleID in beetleIDs)
        {
            GameObject beetle = Instantiate(plot.beetlePrefab);
            beetle.transform.position = plot.plant.transform.position + transforms[beetleID].localPosition;
            beetle.transform.eulerAngles = plot.plant.transform.eulerAngles + transforms[beetleID].localEulerAngles;
            beetle.transform.localScale = transforms[beetleID].localScale;

            beetle.GetComponent<Beetle>().setPlot(plot);
            plot.addToBeetles(beetle, beetleID);
        }
    }

    private void SetFruits(Plot plot, int[] fruitIDs, Transform[] transforms) {
        foreach (int fruitID in fruitIDs)
        {
            GameObject fruit = Instantiate(plot.plant.fruit, plot.plant.transform.position + transforms[fruitID].localPosition, Quaternion.identity);
            fruit.transform.eulerAngles = plot.plant.transform.eulerAngles + transforms[fruitID].localEulerAngles;
            fruit.transform.localScale = transforms[fruitID].localScale;

            fruit.GetComponent<Fruit>().setPlot(plot);
            fruit.GetComponent<InteractionBehaviour>().manager = plot.manager;
            plot.addToFruit(fruit, fruitID);
        }
    }

    private void SetWeeds(Plot plot, SaveObject[] weeds) {
        foreach (SaveObject savedWeed in weeds)
        {
            GameObject weed = Instantiate(plot.weedPrefab);
            weed.transform.position = new Vector3(savedWeed.xPosition, savedWeed.yPosition, savedWeed.zPosition);
            weed.transform.eulerAngles = new Vector3(savedWeed.xRotation, savedWeed.yRotation, savedWeed.zRotation);
            weed.transform.localScale = new Vector3(savedWeed.xScale, savedWeed.yScale, savedWeed.zScale);

            plot.addToWeeds(weed);
        }
    }

    private void LoadOutboxes(SaveOutbox[] savedOutboxes) {
        Outbox[] outboxes = TimeManager.instance.outboxes;
        foreach(Outbox outbox in outboxes) {
            SaveOutbox savedData = FindSavedOutbox(outbox, savedOutboxes);

            // Set previously given gifts for each outbox
            if(savedData != null && savedData.givenGifts != null) {
                foreach(string given in savedData.givenGifts) {
                    Giftable gift = new Giftable();
                    gift.giftName = given;

                    outbox.AddGift(gift);
                }
            }
        }
    }

    private SaveOutbox FindSavedOutbox(Outbox toFind, SaveOutbox[] savedOutboxes) {
        foreach(SaveOutbox saved in savedOutboxes) {
            if (saved.neighbor == toFind.GetLabel()) return saved;
        }
        return null;
    }
}
