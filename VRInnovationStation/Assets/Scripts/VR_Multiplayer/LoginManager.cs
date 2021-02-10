using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class LoginManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField playerName_InputField;

    #region UNITY Methods

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region UI CALLBACK methods

    public void ConnectToPhotonServer()
    {
        //If text is entered, set it as the player's nickname on the photon servers.
        if (playerName_InputField != null)
        {
            PhotonNetwork.NickName = playerName_InputField.text;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    #endregion

    #region PHOTON CALLBACK Methods

    public override void OnConnected()
    {
        //Called when the internet connection is established.
        Debug.Log("<b><color=white>[LoginManager.cs]</color></b> <color=cyan>Server available.</color>");
    }

    public override void OnConnectedToMaster()
    {
        //Called when a user is connected to the Photon server.
        Debug.Log("<b><color=white>[LoginManager.cs]</color></b> <color=cyan>Connected to the Master Photon server with player name: <" + PhotonNetwork.NickName + ">.</color>");

        //Load the home scene using Photon's LoadLevel method. Loads it asynchronously.
        PhotonNetwork.LoadLevel("HomeScene");
    }

    #endregion
}
