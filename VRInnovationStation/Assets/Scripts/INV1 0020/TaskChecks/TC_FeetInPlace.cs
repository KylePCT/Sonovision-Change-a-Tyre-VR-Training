﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TC_FeetInPlace : MonoBehaviourPunCallbacks
{
    //Photon Multiplayer.
    public PhotonView m_photonView;

    //The feet of each moveable arm.
    public GameObject Col_LeftFrontArmPlace;
    public GameObject Col_LeftBackArmPlace;
    public GameObject Col_RightFrontArmPlace;
    public GameObject Col_RightBackArmPlace;

    //UI and Script References.
    public GameObject UI_ProgressTask_2e;
    public GameObject UI_ProgressTask_2v;
    public WheelManager whManager;

    private bool UI_ProgressTaskComplete;

    [HideInInspector]
    public bool AreAllFeetInPlace = false;

    private void Start()
    {
        //Turns off the guides.
        DeactivateAllMeshRenderers();
        UI_ProgressTaskComplete = false;
    }

    //Check if the feet are in the collision to allow the lift to be raised correctly.
    void Update()
    {
        if (Col_LeftFrontArmPlace.GetComponent<TC_FeetInPlace_Single>().IsFootInCollision == true && 
            Col_LeftBackArmPlace.GetComponent<TC_FeetInPlace_Single>().IsFootInCollision == true &&
            Col_RightFrontArmPlace.GetComponent<TC_FeetInPlace_Single>().IsFootInCollision == true &&
            Col_RightBackArmPlace.GetComponent<TC_FeetInPlace_Single>().IsFootInCollision == true)
        {
            //Set variables to true and run multiplayer events.
            AreAllFeetInPlace = true;
            m_photonView.RPC("SetActiveUIElements", RpcTarget.AllBuffered);
            DeactivateAllMeshRenderers();
            Debug.Log("<color=magenta>[TC_FeetInPlace.cs]</color> Lift can now be raised. All four feet are in place.");
        }
        else
        {
            //Set variable to false and also run a check to make sure the simulation isn't at the end by checking if there is a new wheel.
            CheckIfSimIsComplete();
            AreAllFeetInPlace = false;
        }
    }

    //Check if the sim is finished and set progress to 100%.
    public void CheckIfSimIsComplete()
    {
        Debug.Log("<color=magenta>[TC_FeetInPlace.cs]</color> Checking if Sim is complete...");

        //This should be the final simulation task.
        if (whManager.IsNewWheelAttached)
        {
            Debug.Log("<color=magenta>[TC_FeetInPlace.cs]</color> New wheel is attached, arms need moving to complete the simulation.");

            if (Col_LeftFrontArmPlace.GetComponent<TC_FeetInPlace_Single>().IsFootInCollision == false &&
                Col_LeftBackArmPlace.GetComponent<TC_FeetInPlace_Single>().IsFootInCollision == false &&
                Col_RightFrontArmPlace.GetComponent<TC_FeetInPlace_Single>().IsFootInCollision == false &&
                Col_RightBackArmPlace.GetComponent<TC_FeetInPlace_Single>().IsFootInCollision == false)
            {
                Debug.Log("<b><color=magenta>[TC_FeetInPlace.cs]</color> Simulation complete!</b>");
                m_photonView.RPC("SetTo100", RpcTarget.AllBuffered);
            }
        }
    }

    //Set the UI to be updated for all players. Photon Multiplayer.
    [PunRPC]
    void SetActiveUIElements()
    {
        if (UI_ProgressTaskComplete == false)
        {
            UI_ProgressTask_2e.SetActive(true);
            FindObjectOfType<AudioManager>().PlaySound("UI_Complete");
            FindObjectOfType<ProgressChecker>().ChangePercentageTo(25);
            UI_ProgressTaskComplete = true;
        }
    }

    //Update progress to 100% for all players. Photon Multiplayer.
    [PunRPC]
    void SetTo100()
    {
        UI_ProgressTask_2v.SetActive(true);
        FindObjectOfType<AudioManager>().PlaySound("UI_Complete");
        FindObjectOfType<ProgressChecker>().ChangePercentageTo(100);
    }

    //Hide all guides; in its own method for button calls and ease of use.
    public void DeactivateAllMeshRenderers()
    {
        Col_LeftFrontArmPlace.GetComponent<MeshRenderer>().enabled = false;
        Col_LeftBackArmPlace.GetComponent<MeshRenderer>().enabled = false;
        Col_RightFrontArmPlace.GetComponent<MeshRenderer>().enabled = false;
        Col_RightBackArmPlace.GetComponent<MeshRenderer>().enabled = false;
    }
}
