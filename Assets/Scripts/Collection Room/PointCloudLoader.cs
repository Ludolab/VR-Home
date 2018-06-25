using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class PointCloudLoader : MonoBehaviour
{
    private const string PLY_SAVE_PATH = "Assets/Resources/PointClouds/";

    public GameObject pcPrefab;
    public GameObject pool;
    public float poolScale;

    private void Start()
    {
        pool = GameObject.Find("Bottom Water");
        Vector3 poolPos = pool.transform.position;

        string[] paths = Directory.GetFiles(PLY_SAVE_PATH);
        foreach (string path in paths)
        {
            if (Path.GetExtension(path) != ".ply") continue;
            if (Path.GetFileNameWithoutExtension(path) == "head") continue;

            GameObject copy = Instantiate(pcPrefab);
            ParticleSystem.Particle[] plyParticles = PLYFiles.ReadPLY(path);
            KeepParticles keep = copy.transform.Find("PointCloudCopy").GetComponent<KeepParticles>();
            keep.SetParticles(plyParticles);
            Vector2 offset = Random.insideUnitCircle * poolScale;
            copy.transform.position = poolPos + new Vector3(offset.x, 0, offset.y);
            print(copy.transform.position);

            CollectionData.addToClouds(path, copy);
        }
        gameObject.GetComponent<SaveLoadCloud>().Load();
    }
}
