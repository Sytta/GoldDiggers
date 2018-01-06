﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        if (coll != null && this.gameObject.activeSelf && coll.gameObject.GetComponent<GenericUser>().myID 
                                                           != this.gameObject.transform.parent.GetComponent<GenericUser>().myID)
        {
            float[] send = new float[4];
            send[0] = prisonCoord.x;
            send[1] = prisonCoord.y;
            send[2] = prisonCoord.z;
            send[3] = coll.gameObject.GetComponent<GenericUser>().myID;
            coll.gameObject.GetComponent<PhotonView>().RPC("Prison", PhotonTargets.All, send);
        }
	}
}
