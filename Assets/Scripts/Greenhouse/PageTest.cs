using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageTest : MonoBehaviour
{

	public NeighborInfo[] neighbors;
	public GameObject pagePrefab;

	private void Start()
	{
		foreach (NeighborInfo n in neighbors)
		{
			NeighborInfo.LetterInfo[] ls = n.letters;
			foreach (NeighborInfo.LetterInfo l in ls)
			{
				Vector3 position = new Vector3(0, 1, 0);
				GameObject pageObj = Instantiate(pagePrefab, position, pagePrefab.transform.rotation);
				Page page = pageObj.GetComponent<Page>();
				page.SetContents(l.text, n.font, n.fontMaterial, n.paperTexture, n.fontSize);
			}
		}
	}
}
