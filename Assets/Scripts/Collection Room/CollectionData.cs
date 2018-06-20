using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollectionData {

    private static List<GameObject> images = new List<GameObject>(); // Saves a reference to images in the scene.
    private static List<GameObject> sounds = new List<GameObject>(); // Saves a reference to audio in the scene

    public static void addImage(GameObject img) {
        images.Add(img);
    }

    public static List<GameObject> getImages() {
        return images;
    }

    public static void addSound(GameObject sound)
    {
        sounds.Add(sound);
    }

    public static List<GameObject> getSounds()
    {
        return sounds;
    }

}
