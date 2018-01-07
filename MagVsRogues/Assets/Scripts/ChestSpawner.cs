using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : MonoBehaviour {

    [SerializeField] private float GoldMultiplier = 1.0f;
    [SerializeField] private List<Vector3> Positions;
    [SerializeField] private List<Vector3> Rotations;

    // Use this for initialization
    void Start () {
        spawn();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void spawn()
    {
        int i = Random.Range(0, Positions.Capacity);
        Quaternion rotation = Quaternion.Euler(Rotations[i]);
        GameObject newChest = PhotonNetwork.Instantiate("Chest", Positions[i], rotation, 0);
        //newChest.gameObject.transform.eulerAngles = Rotations[i];
        // TODO give powerups !!!
        float s = Random.Range(1.0f, GoldMultiplier);
        newChest.gameObject.GetComponent<ChestController>().gold = (int)(newChest.gameObject.GetComponent<ChestController>().gold * s);
        //newChest.gameObject.transform.localScale *= s;
    }
}
