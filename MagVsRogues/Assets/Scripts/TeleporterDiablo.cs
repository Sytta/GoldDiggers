using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TeleporterDiablo : MonoBehaviour {

    private GameObject coll = null;

    [SerializeField] private Vector3 prisonCoord = new Vector3(4.65f, 2.22f, -15.13f);
    public bool isDown;
    // Use this for initialization
    void Start () {
        

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.gameObject.GetComponent<GenericUser>().myID
                                                   != this.gameObject.transform.parent.GetComponent<GenericUser>().myID)
        {
            coll = other.gameObject;
            Debug.Log(coll.gameObject.GetComponent<GenericUser>().myID);
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

	}

    public void Jail()
    {
        if (coll != null && this.gameObject.activeSelf && coll.gameObject.GetComponent<GenericUser>().myID
                                                   != this.gameObject.transform.parent.GetComponent<GenericUser>().myID)
        {

            coll.GetComponent<Thief>().GetComponent<PhotonView>().RPC("Bust", PhotonTargets.All, null);
            coll.GetComponent<GenericUser>().Teleport(prisonCoord, coll);

            coll = null;
        }
    }
}
