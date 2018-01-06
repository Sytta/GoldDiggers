using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericUser : MonoBehaviour {
    public int myID = -1;

    PhotonView m_PhotonView;

    // Use this for initialization
    void Start() {
        myID = this.gameObject.GetComponent<PhotonView>().ownerId;
        m_PhotonView = GetComponent<PhotonView>();
        DisableScripts();
    }

    // Update is called once per frame
    void Update() {

    }

    void DisableScripts()
    {
        var mageScript = GetComponent<MageScript>().enabled = m_PhotonView.isMine;

    }
}
