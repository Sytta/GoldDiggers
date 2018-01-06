using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericUser : MonoBehaviour {
    public int myID = -1;
    private GameManagerCustom gm;
    [SerializeField] private Transform playerPosition;
    PhotonView m_PhotonView;

    // Use this for initialization
    void Start() {
        myID = this.gameObject.GetComponent<PhotonView>().ownerId;
        m_PhotonView = GetComponent<PhotonView>();
        DisableScripts();

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerCustom>();

        if (myID == gm.Round)
        {
            playerPosition.GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f);
            setMage();
        } else
        {
            playerPosition.GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f);
            setTheif();
        }
    }

    // Update is called once per frame
    void Update() {

    }

    void setMage()
    {
        this.GetComponent<Theif>().enabled = (false);
        this.GetComponent<Mage>().enabled = (true);
        // TODO Swap UI and swap skins
    }

    void setTheif()
    {
        this.GetComponent<Theif>().enabled = (true);
        this.GetComponent<Mage>().enabled = (false);
        // TODO Swap UI and swap skins
    }

    void DisableScripts()
    {
        GetComponent<Mage>().enabled = m_PhotonView.isMine;
        GetComponent<Theif>().enabled = m_PhotonView.isMine;
    }
}
