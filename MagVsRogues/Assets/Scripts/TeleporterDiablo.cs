using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterDiablo : MonoBehaviour {

    private GameObject coll = null;

    [SerializeField] private Vector3 prisonCoord = new Vector3(4.65f, 2.22f, -15.13f);
    // Use this for initialization
    void Start () {
        

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            coll = other.gameObject;
        }
        else
        {
            coll = null;
        }
    }

    void OnTriggerExit(Collider other)
    {
        coll = null;
    }

    // Update is called once per frame
    void Update () {
        if (coll != null && this.gameObject.activeSelf)
        {
            coll.gameObject.GetComponent<Thief>().GetComponent<PhotonView>().RPC("Jail", PhotonTargets.All, prisonCoord);
        }
	}
}
