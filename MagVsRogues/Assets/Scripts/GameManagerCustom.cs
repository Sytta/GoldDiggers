using System;
using System.Collections;
using Photon;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using UnityStandardAssets.Cameras;
using ExitGames.Client.Photon;

public class GameManagerCustom : PunBehaviour
{
    public AbstractTargetFollower Camera;

    [SerializeField] public int Round { get; set; }
    [SerializeField] private GameObject UiScreens;
    [SerializeField] private RectTransform ConnectUiView;
    [SerializeField] private RectTransform WaitingUiView;
    [SerializeField] private RectTransform DisconnectedPanel;
    [SerializeField] private GameObject HUD;

    [SerializeField] private int PlayerCount = 3;
    [SerializeField] public List<GameObject> initialiser;
    [SerializeField] public List<Transform> teleportLocations;
    public GameObject magePlayer;
    public Dictionary<int, GameObject> playerDictionary;

    [SerializeField] private float scoreBoardShowTime = 3.0f;
    public float roundTotalTime = 60.0f;
    public float gameTime = 60.0f;
    public bool runningGameTime = false;

    public int GoldThief1 = 0;
    public int GoldThief2 = 0;
    public int GoldMage = -1;

    public Vector3 ScoringOverall;
    public Vector3 ScoringEndRound;
    int spawnNumber = 2;

    private void OnDestroy()
    {
        PhotonNetwork.Disconnect();
    }

    [PunRPC]
    public void stolenCash(int[] data)
    {
        int ammount = data[0];
        int thief = data[1];

        if (thief == 2)
            GoldThief1 += ammount;
        else
            GoldThief2 += ammount;

        GoldMage = initialiser[5].GetComponent<GoldDistribute>().MageGold - GoldThief1 - GoldThief2;
        Debug.Log(GoldMage);
    }

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
        this.GetComponent<PhotonView>().RPC("setGlobalTime", PhotonTargets.All, gameTime);
    }

    [PunRPC]
    public void RounPrepnextRound(Vector3 v)
    {
        GoldThief1 = 0;
        GoldThief2 = 0;
        GoldMage = initialiser[5].GetComponent<GoldDistribute>().MageGold;
        ScoringEndRound = new Vector3(0, 0, 0);
        //ScoringOverall += v;
    }

    public void RoundReset()
    {
        this.gameObject.GetComponent<PhotonView>().RPC("RounPrepnextRound", PhotonTargets.All, ScoringEndRound);
        if (Round <= 3)
        {
            //Round++;
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
                    player.gameObject.GetComponent<GenericUser>().Teleport(startLocation, player);

                }
                else
                {
                    player.GetComponent<PhotonView>().RPC("setTheif", PhotonTargets.All, playerNumber);
                    float offsetX = spawnNumber == 2 ? -4.5f : 0.5f;
                    Vector3 startLocation = new Vector3(offsetX, -2f, -7.5f);

                    spawnNumber = spawnNumber == 2 ? 1 : 2;
                    player.gameObject.GetComponent<GenericUser>().Teleport(startLocation, player);

                }
            }
            initialiser[5].GetComponent<GoldDistribute>().resetGold(this.gameObject);
        }
        else
        {
        }
    }

    private IEnumerator ShowScores()
    {
        HUD.GetComponent<UIService>().ShowRoundScores();
        yield return new WaitForSeconds(scoreBoardShowTime);
        HUD.GetComponent<UIService>().CloseScores();

        if (Round < 3)
        {
			Round++;
            ReInstantiate();

            if (PhotonNetwork.player.ID == 1)
            {
                RoundReset();
            }
            
        }
        else
        {
            HUD.GetComponent<UIService>().ShowEndGameMessage();
            yield return new WaitForSeconds(scoreBoardShowTime);
            HUD.GetComponent<UIService>().CloseEndGameMessage();
            HUD.GetComponent<UIService>().ShowTotalScores();
        }
    }

    public void Start()
    {


        ScoringOverall = new Vector3(0, 0, 0);
        ScoringEndRound = new Vector3(0, 0, 0);

        RefreshUIViews();
        Round = 1;
        GoldMage = initialiser[5].GetComponent<GoldDistribute>().MageGold;
        playerDictionary = new Dictionary<int, GameObject>();
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

        if (runningGameTime)
        {
            if (gameTime > 0.0f)
                gameTime -= Time.deltaTime;
        }

        if (PhotonNetwork.player.ID == 1)
        {
            if (gameTime <= 0.0f && runningGameTime)
            {
                Debug.Log("END ROUND");
                StopGameTime();
                

                //RoundReset();
                if (Round == 1)
                {
					ScoringEndRound = new Vector3(GoldMage, GoldThief1, GoldThief2);
                }
                else if (Round == 2)
                {
					ScoringEndRound = new Vector3(GoldThief1, GoldMage, GoldThief2);
                }
                else if (Round == 3)
                {
					ScoringEndRound = new Vector3(GoldThief2, GoldThief1, GoldMage);

                }
                this.gameObject.GetComponent<PhotonView>().RPC("sendScoreToAll", PhotonTargets.All, ScoringEndRound);
                //ScoringOverall += ScoringEndRound;
				ResetTime();

            }
        }

    }

    void ReInstantiate()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var player in players)
        {
            var mageNumber = Round;
            if (player.GetComponent<GenericUser>().myID == this.GetComponent<PhotonView>().viewID)
            {
                CreatePlayerObject(player.GetComponent<GenericUser>().myID);
                //this.GetComponent<PhotonView>().ownerId = player.GetComponent<GenericUser>().myID;
                PhotonNetwork.Destroy(player);
                
            }
        }
        
    }


    [PunRPC]
    void sendScoreToAll(Vector3 s)
    {
        ScoringEndRound = s;
        ScoringOverall += s;
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
        HUD.SetActive(PhotonNetwork.inRoom && PhotonNetwork.room.PlayerCount >= PlayerCount);
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
            StartGame();
        }
        else
        {
            Debug.Log("Waiting for another player");
        }
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("Other player arrived");
		RefreshUIViews ();
        if (PhotonNetwork.room.PlayerCount == PlayerCount)
        {
            //////////// START GAME ////////////
            UiScreens.SetActive(false);
            CreatePlayerObject();
            StartGame();
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
        Vector3 position = new Vector3(0f, 2.5f, 0f);

        GameObject newPlayerObject;// = PhotonNetwork.Instantiate("PlayerPrefab", position, Quaternion.identity, 0 );

        Debug.Log("UserId: " + PhotonNetwork.player.ID);
        if (PhotonNetwork.player.ID == Round)
        {
            newPlayerObject = PhotonNetwork.Instantiate("Mage", position, Quaternion.identity, 0);
        } else
        {
            newPlayerObject = PhotonNetwork.Instantiate("Thief", position, Quaternion.identity, 0);
        }

        if (newPlayerObject != null)
            Camera.Target = newPlayerObject.transform;


        var newPlayerId = newPlayerObject.GetComponent<PhotonView>().ownerId;
        Debug.Log("spawned player with id : " + newPlayerId);


    }

    void CreatePlayerObject(int id)
    {
        Vector3 position = new Vector3(0f, 2.5f, 0f);

        GameObject newPlayerObject;// = PhotonNetwork.Instantiate("PlayerPrefab", position, Quaternion.identity, 0 );

        if (id == Round)
        {
            newPlayerObject = PhotonNetwork.Instantiate("Mage", position, Quaternion.identity, 0);
        }
        else
        {
            newPlayerObject = PhotonNetwork.Instantiate("Thief", position, Quaternion.identity, 0);
        }

        if (newPlayerObject != null)
            Camera.Target = newPlayerObject.transform;


        newPlayerObject.GetComponent<GenericUser>().myID = id;
        Debug.Log("spawned player with id : " + id);


    }

    public void FindMage()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var player in players)
        {
            var mageNumber = Round;
            if (player.GetComponent<GenericUser>().myID == mageNumber)
            {
                magePlayer = player;
                return;
            }
        }
    }

    [PunRPC]
    public void setGlobalTime(float t)
    {
        gameTime = t;
        StartCoroutine(ShowScores());
    }

    public void ChangeMageCharacter(int id)
    {
        int currId = 0;
        foreach (var kvp in playerDictionary)
        {
            if (kvp.Value == magePlayer)
            {
                currId = kvp.Key;
            }
        }
        currId = (currId++) % (playerDictionary.Count);

        magePlayer = playerDictionary[currId];
    }
}
