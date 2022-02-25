using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Client.Player;

namespace Networking.LobbyServices
{
    public class SessionSceneManager : MonoBehaviourPunCallbacks
    {
        #region Serializable Fields

        [Tooltip("The default scene to load on when leaving a room.")]
        [SerializeField]
        string defaultScene = string.Empty;

        [Tooltip("List holding all of the potential spawn points")]
        [SerializeField]
        List<SpawnPoint> spawnPoints = new List<SpawnPoint>();


        #endregion

        #region Public Fields

        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;

        #endregion

        #region MonoBehaviour Callbacks

        public void Start()
        {
            if(playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'SessionSceneManager'", this);
            }
            else if(PlayerController.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManager.GetActiveScene());
                PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position, Quaternion.identity, 0);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }

        #endregion

        #region PhotonCallbacks

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.LogFormat("A new player has entered the room: {0}", newPlayer.NickName);

            if(PhotonNetwork.IsMasterClient)
            {
                // In the tutorial, here is where the scene would reload to allow more players - which isn't needed for 2 players only.
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.LogFormat("A player has left the room: {0}", otherPlayer.NickName);

            if(PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("Is the Master Client: {0}", PhotonNetwork.IsMasterClient);

                // In the tutorial, here is where the scene would reload to allow more players - which isn't needed for 2 players only.
            }
        }


        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(defaultScene);
        }

        #endregion

        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region Private Methods

        private void LoadArena()
        {
            if(!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("Trying to load a level but this client is not the master client");
            }
           
            Debug.LogFormat("Loading MultiPlayerRoom");
           
            PhotonNetwork.LoadLevel("MultiPlayerRoom");


        }

        #endregion
    }

}
