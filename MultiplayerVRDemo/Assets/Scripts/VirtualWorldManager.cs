using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class VirtualWorldManager : MonoBehaviourPunCallbacks
{
    //Singleton pattern for referencing outside during instantiating.
    public static VirtualWorldManager Instance;

    private void Awake()
    {
        if (Instance != null & Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void LeaveRoomAndLoadHomeScene()
    {
        PhotonNetwork.LeaveRoom();
    }

    #region PHOTON CALLBACK Methods

    //Notifies the player when another player joins the same room.
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player <" + newPlayer.NickName + "> has joined. Player count: <" + PhotonNetwork.CurrentRoom.PlayerCount + ">.");
    }

    //Called when the player leaves the room.
    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    //Called when the player disconnects.
    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LoadLevel("HomeScene");
    }

    #endregion
}
