using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class GenerateFruits : MonoBehaviour
{

    public GameObject fruitPrefab;

    public GameObject nameGenerator;

    public float[] scales;
    public Color[] colors;

    private GenerateNames nameGen;

    private void Start()
    {
        nameGen = nameGenerator.GetComponent<GenerateNames>();

        float x = -1f;
        for (int i = 0; i < 5; i++)
        {
            GameObject fruit = GenerateFruit();
            fruit.transform.position = new Vector3(x, 1.4f, 1f);
            x += 0.5f;
        }
    }

    public GameObject GenerateFruit()
    {
        GameObject fruit = Instantiate(fruitPrefab);
        int s = Random.Range(0, scales.Length);
        float scale = scales[s];
        fruit.transform.localScale = new Vector3(scale, scale, scale);
        Material fruitMat = fruit.GetComponent<Renderer>().material;

        int c1 = Random.Range(0, colors.Length);
        int c2 = Random.Range(0, colors.Length);
        Color color1 = colors[c1]; //TODO: add a little random adjustment
        Color color2 = colors[c2];
        fruitMat.SetColor("_Color1", color1);
        fruitMat.SetColor("_Color2", color2);

        string name = nameGen.GenerateName();
        TextMesh textMesh = fruit.transform.Find("Name").GetComponent<TextMesh>();
        textMesh.text = name;

        return fruit;
    }
}
