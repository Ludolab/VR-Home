using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class PointCloudLoader : MonoBehaviour
{
    private const string PLY_SAVE_PATH = "Assets/Resources/PointClouds/";
    private const float HEIGHT_DIFF = 0.05f;

    public GameObject pcPrefab;
    
    private void Start()
    {
        float height = transform.position.y;

        string[] paths = Directory.GetFiles(PLY_SAVE_PATH);
        foreach (string path in paths)
        {
            if (Path.GetExtension(path) != ".ply") continue;
            if (Path.GetFileNameWithoutExtension(path) == "head") continue;

            GameObject copy = Instantiate(pcPrefab);
            ParticleSystem.Particle[] plyParticles = PLYFiles.ReadPLY(path);
            KeepParticles keep = copy.transform.Find("PointCloudCopy").GetComponent<KeepParticles>();
            keep.SetParticles(plyParticles);
            copy.transform.position = new Vector3(transform.position.x, height, transform.position.z);
            height += HEIGHT_DIFF;
        }
    }
}
