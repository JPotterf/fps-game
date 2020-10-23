using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.Potterf.FpsGame { 
    public class Launcher : MonoBehaviourPunCallbacks
    {
        public void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            Connect();
        }

        public override void OnConnectedToMaster()
        {
            Join();
            base.OnConnectedToMaster();
        }

        public override void OnJoinedRoom()
        {
            StartGame();
            base.OnJoinedRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Create();
            base.OnJoinRandomFailed(returnCode, message);
        }

        public void Connect()
        {
            PhotonNetwork.GameVersion = "0.0.0";
            PhotonNetwork.ConnectUsingSettings();
        }

        public void Create()
        {
            PhotonNetwork.CreateRoom("");
        }

        public void Join()
        {
            //simple matchmaking, no extra params
            PhotonNetwork.JoinRandomRoom();
        }
        public void StartGame()
        {
            //first player is host, all other client connections join the existing room... for now
            if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.LoadLevel(1);
            }
        }
    }
}