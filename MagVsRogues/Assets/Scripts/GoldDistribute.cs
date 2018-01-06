using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldDistribute : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject[] chests = GameObject.FindGameObjectsWithTag("Chest");
        int totalGold = 0;
        for(int i = 0; i < chests.Length; ++i)
        {
            totalGold += chests[i].GetComponent<ChestController>().gold;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
