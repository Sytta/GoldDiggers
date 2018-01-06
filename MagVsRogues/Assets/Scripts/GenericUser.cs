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
        } else
        {
            playerPosition.GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f);
        }
    }

    // Update is called once per frame
    void Update() {

    }

    void DisableScripts()
    {
        var mageScript = GetComponent<MageScript>().enabled = m_PhotonView.isMine;

    }
}
