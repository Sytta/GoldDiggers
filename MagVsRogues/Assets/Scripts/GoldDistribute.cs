using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldDistribute : MonoBehaviour {

    [SerializeField] private int MageGold = 1000;
    // Use this for initialization
    void Start () {
        GameObject[] chests = GameObject.FindGameObjectsWithTag("Chest");
        int totalGold = 0;
        for(int i = 0; i < chests.Length; ++i)
        {
            totalGold += chests[i].GetComponent<ChestController>().gold;
        }
        float multiplier = MageGold * 1.0f / totalGold;
        int total = MageGold;
        for (int i = 0; i < chests.Length - 1; ++i)
        {
            chests[i].GetComponent<ChestController>().gold = (int)(chests[i].GetComponent<ChestController>().gold * multiplier);
            total -= chests[i].GetComponent<ChestController>().gold;

            chests[i].GetComponent<PhotonView>().RPC("initGold", PhotonTargets.All, chests[i].GetComponent<ChestController>().gold);
        }

        chests[chests.Length - 1].GetComponent<ChestController>().gold = total;
        chests[chests.Length - 1].GetComponent<PhotonView>().RPC("initGold", PhotonTargets.All, total);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
