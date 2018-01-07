﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericUser : MonoBehaviour {
    public int myID = -1;
    private GameManagerCustom gm;
    [SerializeField] private Transform playerPosition;
    PhotonView m_PhotonView;

    // Use this for initialization
    void Start()
    {
        myID = this.gameObject.GetComponent<PhotonView>().ownerId;
        m_PhotonView = GetComponent<PhotonView>();
        DisableScripts();

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerCustom>();
        if (m_PhotonView.isMine)
        {
            if (myID == gm.Round)
            {
                playerPosition.GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f);
                setMage();
            }
            else
            {
                playerPosition.GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f);
                setTheif();
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void setMage()
    {
        this.GetComponent<Thief>().enabled = (false);
        this.GetComponent<Mage>().enabled = (true);
        // TODO Swap UI and swap skins
    }

    public void setTheif()
    {
        this.GetComponent<Thief>().enabled = (true);
        this.GetComponent<Thief>().SpawnThief(myID);
        this.GetComponent<Mage>().enabled = (false);
        // TODO Swap UI and swap skins
    }

    void DisableScripts()
    {
        GetComponent<Mage>().enabled = m_PhotonView.isMine;
        GetComponent<Thief>().enabled = m_PhotonView.isMine;
    }

    [PunRPC]
    void Prison(float[] p)
    {
        if ((int)(p[3]) == this.gameObject.GetComponent<GenericUser>().myID)
        {
            Vector3 pos = new Vector3(p[0], p[1], p[2]);
            this.gameObject.transform.position = pos;
        }
    }
}
