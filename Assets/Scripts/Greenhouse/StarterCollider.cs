using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterCollider : MonoBehaviour {
    public GameObject myStarter;

	private void Update()
	{
        this.gameObject.layer = 9; //Seedling
	}
}
