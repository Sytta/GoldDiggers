using System;
using System.Collections;
using Photon;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Collections.Generic;

using ExitGames.Client.Photon;

public class GameManagerCustom : PunBehaviour
{
    public CameraSimple Camera;

    [SerializeField] public int Round { get; set; }
    [SerializeField] private GameObject UiScreens;
    [SerializeField] private RectTransform ConnectUiView;
    [SerializeField] private RectTransform WaitingUiView;
    [SerializeField] private RectTransform DisconnectedPanel;

    [SerializeField] private int PlayerCount = 3;
    [SerializeField] private List<GameObject> initialiser;
    [SerializeField] public List<Transform> teleportLocations;
    public GameObject magePlayer;
    public Dictionary<int,GameObject> playerDictionary;

    public float roundTotalTime = 60.0f;
    public float gameTime = 60.0f;
    public bool runningGameTime = false;
    int spawnNumber = 2;


    public void StartGame()
    {
        runningGameTime = true;
    }

    public void StopGameTime()
    {
        runningGameTime = false;
    }
    public void ResetTime()
    {
        StopGameTime();
        gameTime = roundTotalTime;
    }

    public void RoundReset()
    {
        Round = (Round)%3 +1;
        Debug.Log("Starting round : " + Round);
        StartGame();
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            var mageNumber = Round;
            var playerGenericUser = player.GetComponent<GenericUser>();
            var playerNumber = playerGenericUser.myID;
            if (playerNumber == mageNumber)
            {
                magePlayer = player;
                player.GetComponent<PhotonView>().RPC("setMage", PhotonTargets.All, playerNumber);
                Vector3 startLocation = new Vector3(0f, 2.5f, 0f);
                this.gameObject.GetComponent<GenericUser>().Teleport(startLocation, player);

            }
            else
            {
                player.GetComponent<PhotonView>().RPC("setTheif", PhotonTargets.All, playerNumber);
                float offsetX = spawnNumber == 2 ? -4.5f : 0.5f;
                Vector3 startLocation = new Vector3(offsetX, -2f, -7.5f);

                spawnNumber = spawnNumber == 2 ? 1 : 2;
                this.gameObject.GetComponent<GenericUser>().Teleport(startLocation, player);

            }
        }
    }

    public void Start()
    {
        RefreshUIViews();
        Round = 1;
        playerDictionary = new Dictionary<int, GameObject>();
        StartGame();
    }

    public void Update()
    {
        // Check if we are out of context, which means we likely got back to the demo hub.
        if (this.DisconnectedPanel == null)
        {
            Destroy(this.gameObject);
        }

        // for debugging, it's useful to have a few actions tied to keys:
        if (Input.GetKeyUp(KeyCode.L))
        {
            PhotonNetwork.LeaveRoom();
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            PhotonNetwork.ConnectUsingSettings(null);
            PhotonHandler.StopFallbackSendAckThread();
        }


        if (!PhotonNetwork.inRoom)
        {
            return;
        }

        // disable the "reconnect panel" if PUN is connected or connecting
        if (PhotonNetwork.connected && this.DisconnectedPanel.gameObject.GetActive())
        {
            this.DisconnectedPanel.gameObject.SetActive(false);
        }
        if (!PhotonNetwork.connected && !PhotonNetwork.connecting && !this.DisconnectedPanel.gameObject.GetActive())
        {
            this.DisconnectedPanel.gameObject.SetActive(true);
        }


      

    }

    #region Core Gameplay Methods
    private void UpdatePlayerTexts()
    {
        
    }
    #endregion


    #region Handling Of Buttons
    public void OnClickConnect()
    {
        PhotonNetwork.ConnectUsingSettings(null);
        PhotonHandler.StopFallbackSendAckThread();  // this is used in the demo to timeout in background!
    }

    public void OnClickReConnectAndRejoin()
    {
        PhotonNetwork.ReconnectAndRejoin();
        PhotonHandler.StopFallbackSendAckThread();  // this is used in the demo to timeout in background!
    }

    #endregion

    void RefreshUIViews()
    {
        ConnectUiView.gameObject.SetActive(!PhotonNetwork.inRoom);
        WaitingUiView.gameObject.SetActive(PhotonNetwork.inRoom && PhotonNetwork.room.PlayerCount < PlayerCount);
    }


    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom()");



        RefreshUIViews();
    }

    public override void OnJoinedRoom()
    {
        RefreshUIViews();

        if (PhotonNetwork.room.PlayerCount >= PlayerCount)
        {
            //////////// START GAME ////////////
            UiScreens.SetActive(false);
            CreatePlayerObject();
            foreach (var go in initialiser)
            {
                if (go != null)
                    go.SetActive(true);
            }
       
        }
        else
        {
            Debug.Log("Waiting for another player");
        }
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("Other player arrived");

        if (PhotonNetwork.room.PlayerCount == PlayerCount)
        {
            //////////// START GAME ////////////
            UiScreens.SetActive(false);
            CreatePlayerObject();
        }
    }


    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log("Other player disconnected! " + otherPlayer.ToStringFull());
    }


    public override void OnConnectionFail(DisconnectCause cause)
    {
        this.DisconnectedPanel.gameObject.SetActive(true);
    }


    /////// RPG //////

    void CreatePlayerObject()
    {
        Vector3 position = new Vector3( 0f, 2.5f, 0f );

        //GameObject newPlayerObject = PhotonNetwork.Instantiate("PlayerPrefab", position, Quaternion.identity, 0 );

        GameObject newPlayerObject = PhotonNetwork.Instantiate("Mage", position, Quaternion.identity, 0);

        if (newPlayerObject != null)
            Camera.Target = newPlayerObject.transform;


        var newPlayerId = newPlayerObject.GetComponent<PhotonView>().ownerId;
        Debug.Log("spawned player with id : " + newPlayerId);
      

    }

    public void FindMage()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");

        foreach(var player in players)
        {
            var mageNumber = Round;
            if(player.GetComponent<GenericUser>().myID == mageNumber)
            {
                magePlayer = player;
                return;
            }
        }
    }

    public void ChangeMageCharacter(int id)
    {
        int currId = 0;
        foreach(var kvp in playerDictionary)
        {
            if(kvp.Value == magePlayer)
            {
                currId = kvp.Key;
            }
        }
        currId = (currId++) % (playerDictionary.Count);

        magePlayer = playerDictionary[currId];
    }
}
