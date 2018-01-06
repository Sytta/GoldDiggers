using System;
using System.Collections;
using Photon;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

using ExitGames.Client.Photon;

public class GameManagerCustom : PunBehaviour
{
    public CameraSimple Camera;

    [SerializeField] private GameObject UiScreens;
    [SerializeField] private RectTransform ConnectUiView;
    [SerializeField] private RectTransform WaitingUiView;
    [SerializeField] private RectTransform DisconnectedPanel;

    int PlayerCount = 2;

    public void Start()
    {
        RefreshUIViews();
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

        // TODO CHANGE
        if (PhotonNetwork.room.PlayerCount > 2)
        {
            ////////// TURN MANAGEMENT /////////////

        }

    }

    #region Core Gameplay Methods
    public void EndGame()
    {
        Debug.Log("EndGame");
    }

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
        WaitingUiView.gameObject.SetActive(PhotonNetwork.inRoom && PhotonNetwork.room.PlayerCount != PlayerCount);
    }


    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom()");



        RefreshUIViews();
    }

    public override void OnJoinedRoom()
    {
        RefreshUIViews();

        if (PhotonNetwork.room.PlayerCount == PlayerCount)
        {
            //////////// START GAME ////////////
            UiScreens.SetActive(false);
            CreatePlayerObject();
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
        Vector3 position = new Vector3( 33.5f, 1.5f, 20.5f );

        GameObject newPlayerObject = PhotonNetwork.Instantiate("PlayerPrefab", position, Quaternion.identity, 0 );

        if (newPlayerObject != null)
            Camera.Target = newPlayerObject.transform;
    }

}
