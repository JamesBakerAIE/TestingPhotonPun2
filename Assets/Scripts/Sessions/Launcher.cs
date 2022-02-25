using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Networking.LobbyServices
{
    /// <summary>
    /// Launcher contains functionality to connect to the Photon Cloud servers & connecting each player to the server.
    /// </summary>
    public class Launcher : MonoBehaviourPunCallbacks
    {

        #region Serializable Fields

        [Tooltip("The maximum number of players per room. When the room is full, it can't be joined by new players. So a new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 2;

        [Tooltip("The UI Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;

        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;

        #endregion

        #region Private Fields

        string gameVersion = "0.01";

        bool isConnecting = false;
        #endregion

        #region MonoBehaviour CallBacks

        private void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);

        }

        private void Update()
        {

        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to master server");

            if (isConnecting)
            {
                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            if(progressLabel != null)
            {
                progressLabel.SetActive(false);
            }
            
            if(controlPanel != null)
            {
                controlPanel.SetActive(true);
            }
            isConnecting = false;
            Debug.LogWarningFormat("Disconnected from master server with reason {0}", cause);

        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("No random room available, creating a room.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. Create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Connected to room");

            if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("Loading MultiPlayerRoom");
                PhotonNetwork.LoadLevel("MultiPlayerRoom");
            }
        }

        #endregion

        #region Public Methods

        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);


            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        #endregion

    }
}
