using System.Collections;
using System.Collections.Generic;
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
                GenerateFruit(pos);
                x += 0.5f;
            }
            y += 0.5f;
        }

		/*Vector3 potPos = GameObject.Find("PlantableSpot").transform.position;
		GenerateFruit(potPos);*/
    }

    public GameObject GenerateFruit(Vector3 pos)
    {
        int s = Random.Range(0, scales.Length);
        float scale = scales[s];
        float stretchScale = scale * Random.Range(0.8f, 2.0f);

		int c1 = Random.Range(0, colors.Length);
        int c2 = Random.Range(0, colors.Length);
        Color color1 = colors[c1];
        Color color2 = colors[c2];

		GameObject fruitObj = Instantiate(fruitPrefab, pos, Quaternion.identity);
		GameObject fruit = fruitObj.transform.Find("Fruit").gameObject;
		Fruit fruitInfo = fruit.GetComponent<Fruit>();

		string name = nameGen.GenerateName();
		TextMesh textMesh = fruitObj.transform.Find("Name").GetComponent<TextMesh>();
		textMesh.text = name;

		float lifetime = Random.Range(10.0f, 20.0f);
		
		Genome g = new Genome
		{
			name = name,
			color1 = color1,
			color2 = color2,
			scale = scale,
			stretchScale = stretchScale,
			lifetime = lifetime
		};
		fruitInfo.SetGenome(g);

        return fruitObj;
    }
}
