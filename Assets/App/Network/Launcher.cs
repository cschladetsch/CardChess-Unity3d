using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace App.Network
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    
    /// <summary>
    /// What this class does and how it works with other classes.
    /// </summary>
    public class Launcher
        : MonoBehaviourPunCallbacks
    {
        public string RoomName = "1000";
        public string GameVersion = "0.1";
        public int JoinRetryDelaySeconds = 5;

        private bool _joining;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            Connect();
        }

        private void Connect()
        {
            if (PhotonNetwork.IsConnected)
            {
                JoinRoom();
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.GameVersion = GameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        private void JoinRoom()
        {
            PhotonNetwork.CreateRoom(RoomName, new RoomOptions {MaxPlayers = 4});
        }
        
        private IEnumerator WaitAndConnectToRoom()
        {
            yield return new WaitForSeconds(JoinRetryDelaySeconds);
            PhotonNetwork.JoinRoom(RoomName);
        }

        #region PunCallbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster()");

            JoinRoom();
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("OnDisconnected(): {0}", cause);

            Connect();
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log($"Failed to join room: {returnCode}: {message}");
            if (_joining)
                StartCoroutine(WaitAndConnectToRoom());
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Launcher: OnJoinedRoom()");
            Debug.Log($"Using Region={PhotonNetwork.CloudRegion}");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log($"Player {newPlayer} joined.");
        }
        #endregion
    }
}

