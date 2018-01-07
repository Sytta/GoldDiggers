using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GoldDistribute : Photon.MonoBehaviour
{
    [SerializeField] public int MageGold = 1000;
    // Use this for initialization
    void Start() {
        GameObject[] chests = GameObject.FindGameObjectsWithTag("Chest");
        int totalGold = 0;
        for (int i = 0; i < chests.Length; ++i)
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
        if (chests.Length - 1 > 0)
        {
            chests[chests.Length - 1].GetComponent<ChestController>().gold = total;
            chests[chests.Length - 1].GetComponent<PhotonView>().RPC("initGold", PhotonTargets.All, total);
        }
    }

    [PunRPC]
    public void destroyChests()
    {
        GameObject[] chests = GameObject.FindGameObjectsWithTag("Chest");
        for (int i = 0; i < chests.Length; ++i)
        {
            chests[i].gameObject.tag = "Untagged";
            if (chests[i].gameObject.GetComponent<PhotonView>().isMine)
                PhotonNetwork.Destroy(chests[i].gameObject);
        }
    }

    public void resetGold(GameObject go)
    {

        this.gameObject.GetComponent<PhotonView>().RPC("destroyChests", PhotonTargets.All, null);
        this.gameObject.transform.parent.GetComponent<GameManagerCustom>().initialiser[0].GetComponent<ChestSpawner>().spawn();
        this.gameObject.transform.parent.GetComponent<GameManagerCustom>().initialiser[1].GetComponent<ChestSpawner>().spawn();
        this.gameObject.transform.parent.GetComponent<GameManagerCustom>().initialiser[2].GetComponent<ChestSpawner>().spawn();
        this.gameObject.transform.parent.GetComponent<GameManagerCustom>().initialiser[3].GetComponent<ChestSpawner>().spawn();
        this.gameObject.transform.parent.GetComponent<GameManagerCustom>().initialiser[4].GetComponent<ChestSpawner>().spawn();

        Start();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
