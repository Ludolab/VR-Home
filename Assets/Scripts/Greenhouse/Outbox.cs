using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outbox : MonoBehaviour
{

	public GameObject labelPrefab;
	public Transform labelTransform;
	public Transform paperTransform;

	private GameObject label;
	private Neighbor owner;

	private List<Giftable> gifts = new List<Giftable>();

	public void SetOwner(Neighbor neighbor)
	{
		owner = neighbor;

		NeighborInfo info = neighbor.info;
		string text = string.IsNullOrEmpty(info.labelOverride) ? info.name : info.labelOverride;

		GameObject label = Instantiate(labelPrefab, labelTransform);
		GameObject labelText = label.transform.Find("text").gameObject;
		TextMesh labelMesh = labelText.GetComponent<TextMesh>();
		Renderer labelRend = labelText.GetComponent<Renderer>();

		labelMesh.text = text;
		labelMesh.font = info.font;
		labelRend.material = info.fontMaterial;

		this.label = label;
	}

	public Neighbor GetOwner()
	{
		return owner;
	}

	public void AddGift(Giftable g)
	{
		gifts.Add(g);
	}

	public void RemoveGift(Giftable g)
	{
		gifts.Remove(g);
	}

	public string[] ClearFruit()
	{
		List<string> giftNames = new List<string>();
		foreach (Giftable g in gifts)
		{
			giftNames.Add(g.name);
			//Destroy(g.gameObject);
		}
		gifts.Clear();
		return giftNames.ToArray();
	}

    public string[] GetGiftNames() {
        List<string> g = new List<string>();
        foreach(Giftable gift in gifts) {
            g.Add(gift.giftName);
        }
        return g.ToArray();
    }

	public string GetLabel()
	{
		if (label == null) return null;

		return label.transform.Find("text").GetComponent<TextMesh>().text;
	}
}
