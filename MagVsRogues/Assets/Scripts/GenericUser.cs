using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericUser : MonoBehaviour {
    public int myID = -1;
    public int currentGold = 0;
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
                setMage(myID);
                currentGold = gm.initialiser[5].GetComponent<GoldDistribute>().MageGold;
            }
            else
            {
                playerPosition.GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f);
                setTheif(myID);
            }
        }
        else
            DisableScripts();
    }

    // Update is called once per frame
    void Update() {

    }

    [PunRPC]
    public void setMage(int player)
    {
        EventManager.Instance.QueueEvent(new OnPowerUpReset());
        EventManager.Instance.QueueEvent(new OnPowerUpCreated(PowerUpType.Jail));
        EventManager.Instance.QueueEvent(new OnPowerUpCreated(PowerUpType.Infrared, 15.0f));

        if(player == myID)
        {
            Mage mage = GetComponent<Mage>();
            Thief thief = GetComponent<Thief>();
            if (thief != null)
                this.GetComponent<Thief>().enabled = (false);
            if (mage != null)
                this.GetComponent<Mage>().enabled = (true);
        }
        // TODO Swap UI and swap skins
    }


    [PunRPC]
    public void setTheif(int player)
    {
        EventManager.Instance.QueueEvent(new OnPowerUpReset());

        if (player == myID)
        {
            Mage mage = GetComponent<Mage>();
            Thief thief = GetComponent<Thief>();
            if (mage != null)
                this.GetComponent<Mage>().enabled = (false);

            if (thief != null)
            {
                this.GetComponent<Thief>().enabled = (true);
                this.GetComponent<Thief>().SpawnThief(myID);
            }
            
        }
        // TODO Swap UI and swap skins
    }

    void DisableScripts()
    {

        Mage mage = GetComponent<Mage>();
        Thief thief = GetComponent<Thief>();
        if (mage != null)
            mage.enabled = m_PhotonView.isMine;
        if (thief != null)
            GetComponent<Thief>().enabled = m_PhotonView.isMine;
    }

    public void Teleport(Vector3 v, GameObject go)
    {

        float[] send = new float[4];
        send[0] = v.x;
        send[1] = v.y;
        send[2] = v.z;
        send[3] = go.GetComponent<GenericUser>().myID;

        this.gameObject.GetComponent<PhotonView>().RPC("Prison", PhotonTargets.All, send);
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
