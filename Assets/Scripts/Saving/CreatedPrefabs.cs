using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CreatedPrefabs {

    private static Dictionary<int, string> createdObj = new Dictionary<int, string>();

    public static void addToCreated(int created, string from) {
        createdObj.Add(created, from);
    }

    public static Dictionary<int, string> getCreatedObj() {
        return createdObj;
    }

}

public class SaveCreated {
    
}
