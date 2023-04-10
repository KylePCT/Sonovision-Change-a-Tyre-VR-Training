using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    TextMeshProUGUI OccupancyRateText_BronzeTier;

    [SerializeField]
    TextMeshProUGUI OccupancyRateText_SilverTier;

    [SerializeField]
    TextMeshProUGUI OccupancyRateText_GoldTier;

    string mapType;

    // Start is called before the first frame update
    void Start()
    {
        //Synchronise the scene for all players in that room.
        PhotonNetwork.AutomaticallySyncScene = true;
        
        //If not connected, reconnect.
        if(!PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        else
        {
            PhotonNetwork.JoinLobby();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region UI CALLBACK Methods

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    //Public methods to enter the two custom maps.
    public void OnEnterRoomButtonClicked_Bronze()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_BRONZE;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    public void OnEnterRoomButtonClicked_Silver()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_SILVER;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    public void OnEnterRoomButtonClicked_Gold()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_GOLD;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    #endregion

    #region PHOTON CALLBACK Methods

    //Called when connected to master.
    public override void OnConnectedToMaster()
    {
        Debug.Log("<color=cyan>[RoomManager.cs] Reconnected.</color>");
        PhotonNetwork.JoinLobby();
    }

    //Called when the program has failed to join a room.
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //Debugs the error message.
        Debug.Log(message);
        CreateAndJoinRoom();
    }

    //Called when joining a lobby.
    public override void OnJoinedLobby()
    {
        Debug.Log("<color=cyan>[RoomManager.cs] Joined to lobby.</color>");
    }

    //Called when a room is created.
    public override void OnCreatedRoom()
    {
        Debug.Log("<color=cyan>[RoomManager.cs] A room has been created with the name: <" + PhotonNetwork.CurrentRoom.Name + ">.</color>");
    }

    //Called when joining a room.
    public override void OnJoinedRoom()
    {
        Debug.Log("<color=cyan>[RoomManager.cs] The local player <" + PhotonNetwork.NickName + "> has joined <" + PhotonNetwork.CurrentRoom.Name + ">. Player count: <" + PhotonNetwork.CurrentRoom.PlayerCount + ">.</color>");

        //If the map/room has a key set.
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(MultiplayerVRConstants.MAP_TYPE_KEY))
        {
            object mapType;

            //Get the map type and name.
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(MultiplayerVRConstants.MAP_TYPE_KEY, out mapType))
            {
                Debug.Log("<color=cyan>[RoomManager.cs] Joined room with the map: <" + (string)mapType + ">.</color>");

                //If the map name is _____...
                if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_BRONZE)
                {
                    //Load outdoor scene.
                    PhotonNetwork.LoadLevel("World_Bronze");
                }
                else if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_SILVER)
                {
                    //Load schools scene.
                    PhotonNetwork.LoadLevel("World_Silver");
                }
                else if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_GOLD)
                {
                    //Load schools scene.
                    PhotonNetwork.LoadLevel("World_Gold");
                }
            }
        }
    }

    //Called when a player enters the same room as the user.
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("<color=cyan>[RoomManager.cs] Player <" + newPlayer.NickName + "> has joined. Player count: <" + PhotonNetwork.CurrentRoom.PlayerCount + ">.</color>");
    }

    //Called whenever the room listing is updated. => When a room is created; when a room is joined.
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (roomList.Count == 0)
        {
            //There are no rooms.
            OccupancyRateText_BronzeTier.text = 0 + "/" + 20;
            OccupancyRateText_SilverTier.text = 0 + "/" + 20;
            OccupancyRateText_GoldTier.text = 0 + "/" + 20;
        }

        //Update the player counts.
        foreach (RoomInfo room in roomList)
        {
            if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_BRONZE))
            {
                //Update the Bronze map player count.
                OccupancyRateText_BronzeTier.text = room.PlayerCount + "/" + 20;
            }

            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_SILVER))
            {
                //Update the Silver map player count.
                OccupancyRateText_SilverTier.text = room.PlayerCount + "/" + 20;
            }

            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_GOLD))
            {
                //Update the Gold map player count.
                OccupancyRateText_GoldTier.text = room.PlayerCount + "/" + 20;
            }
        }
    }

    #endregion

    #region PRIVATE Methods

    void CreateAndJoinRoom()
    {
        string randomRoomName = "Room_" + mapType + "_" + Random.Range(0, 9001);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20; //Limit of Photon Free.

        //Holds the room properties to show to the lobby => where Photon organises the rooms.
        string[] roomPropsInLobby = {MultiplayerVRConstants.MAP_TYPE_KEY}; //See the MultiplayerVRConstants.cs to edit this.

        //Create a hashtable and add data in pairs.
        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };
        //See the MultiplayerVRConstants.cs to edit the MAP_TYPE_VALUE which can allow loading of different scenes.

        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
        roomOptions.CustomRoomProperties = customRoomProperties;

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }

    #endregion
}
