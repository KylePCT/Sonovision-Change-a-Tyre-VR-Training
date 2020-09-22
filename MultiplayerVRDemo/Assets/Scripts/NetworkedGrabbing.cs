﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkedGrabbing : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    private PhotonView m_photonView;
    public Rigidbody rb; //For access to the Kinematic options.
    public bool isBeingHeld = false; //Keep track of the object held.

    //Grabbed object string storage.
    string grabbedName;
    string objectName;

    //Start is called before the first frame update.
    private void Awake()
    {
        //Get the PhotonView component.
        m_photonView = GetComponent<PhotonView>();

        //Set the names of the presumed grabbed object.
        Debug.Log("<" + m_photonView.gameObject.name + "> could be <" + gameObject.name + ">.");
        grabbedName = m_photonView.gameObject.name;
        objectName = gameObject.name;
    }

    void Start()
    {
        //Get the rigidbody.
        rb = GetComponent<Rigidbody>();
    }

    [PunRPC]
    //Update is called once per frame.
    void Update()
    {
        if (!isBeingHeld) //If the object is not being held.
        {
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
        }

        if (isBeingHeld) //If the object is being held.
        {
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }

    //When object is grabbed...
    public void OnSelectEnter()
    {
        //Calls the RPC method across players in the room.
        m_photonView.RPC("StartNetworkGrabbing", RpcTarget.AllBuffered);

        //If the owner is the local player...
        if (m_photonView.Owner == PhotonNetwork.LocalPlayer)
        {
            Debug.Log("Ownership not transferred. It is already owned by the local player.");
        }

        //If it's anyone else...
        else
        {
            TransferOwnership();
        }
    }

    //When object is released...
    public void OnSelectExit()
    {
        //When an object is no longer selected, call the StopNetworkGrabbing method to all players in the same room.
        m_photonView.RPC("StopNetworkGrabbing", RpcTarget.AllBuffered);
    }

    private void TransferOwnership()
    {
        m_photonView.RequestOwnership();
    }

    //When ownership is requested...
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        //Does a check for the code to run only the grabbed object and not any others in the scene.
        //If the grabbed object is not the one we are interacting with...
        if (targetView != m_photonView)
        {
            return; //Go back. Never to be seen again. Nope. No comprendé. Stops anymore code running in this method.
        }

        Debug.Log("Ownership requested for: <" + targetView.name + "> from Player: <" + requestingPlayer.NickName + ">.");
        m_photonView.TransferOwnership(requestingPlayer);
    }

    //Once the ownership transfer is complete...
    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("Ownership transfer complete to Player: <" + targetView.Owner.NickName + ">.");

        if (photonView.IsMine)
        {
            //Calls the RPC method across players in the room.
            m_photonView.RPC("StartNetworkGrabbing", RpcTarget.AllBuffered);
        }
    }

    [PunRPC] //This will be sent to all remotely connected players and update them on this.
    public void StartNetworkGrabbing(string whatIsGrabbed)
    {
        //If the grabbed object is being held, set it to true.
        if (whatIsGrabbed == objectName)
        {
            isBeingHeld = true;
        }

    }

    [PunRPC] //This will be sent to all remotely connected players and update them on this.
    public void StopNetworkGrabbing(string whatIsGrabbed)
    {
        //If the grabbed object is being held, set it to false and drop it.
        if (whatIsGrabbed == objectName)
        {
            isBeingHeld = false;
        }
    }
}
