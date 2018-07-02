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

        float y = 1.4f;
        for (int i = 0; i < 2; i++)
        {
            float x = -1f;
            for (int j = 0; j < 5; j++)
            {
                Vector3 pos = new Vector3(x, y, 1f);
                GameObject fruit = GenerateFruit(pos);
                x += 0.5f;
            }
            y += 0.5f;
        }

    }

    public GameObject GenerateFruit(Vector3 pos)
    {
        GameObject fruitObj = Instantiate(fruitPrefab, pos, Quaternion.identity);
        GameObject fruit = fruitObj.transform.Find("Fruit").gameObject;

        int s = Random.Range(0, scales.Length);
        float scale = scales[s];
        float stretchScale = scale * Random.Range(0.8f, 2.0f);
        fruit.transform.localScale = new Vector3(scale, stretchScale, scale);
        //fruit.transform.rotation = Random.rotation;

        Material fruitMat = fruit.GetComponent<Renderer>().material;
        int c1 = Random.Range(0, colors.Length);
        int c2 = Random.Range(0, colors.Length);
        Color color1 = colors[c1]; //TODO: add a little random adjustment
        Color color2 = colors[c2];
        fruitMat.SetColor("_Color1", color1);
        fruitMat.SetColor("_Color2", color2);

        string name = nameGen.GenerateName();
        TextMesh textMesh = fruitObj.transform.Find("Name").GetComponent<TextMesh>();
        textMesh.text = name;

        return fruitObj;
    }
}
