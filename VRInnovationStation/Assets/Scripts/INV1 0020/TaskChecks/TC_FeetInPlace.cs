using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TC_FeetInPlace : MonoBehaviourPunCallbacks
{
    //Photon Multiplayer.
    public PhotonView m_photonView;

    //The feet of each moveable arm.
    [Header("Chassis Collisions")]
    public GameObject Col_LeftFrontArmPlace;
    public GameObject Col_LeftBackArmPlace;
    public GameObject Col_RightFrontArmPlace;
    public GameObject Col_RightBackArmPlace;

    //The origin collisions.
    [Header("Origin Collisions")]
    public GameObject Col_LeftFrontArmPlaceDefault;
    public GameObject Col_LeftBackArmPlaceDefault;
    public GameObject Col_RightFrontArmPlaceDefault;
    public GameObject Col_RightBackArmPlaceDefault;

    //UI and Script References.
    [Header("References")]
    public GameObject UI_ProgressTask_2e;
    public GameObject UI_ProgressTask_2v;
    public WheelManager whManager;

    private bool UI_ProgressTaskComplete = false;
    public bool AreAllFeetInPlace = false;

    private void Start()
    {
        //Turns off the guides.
        DeactivateAllMeshRenderers();
        TurnOffSecondCollisions();

        UI_ProgressTaskComplete = false;
    }

    //Check if the feet are in the collision to allow the lift to be raised correctly.
    void Update()
    {
        //################################# Should probably throw this through an array and do a foreach...
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
            AreAllFeetInPlace = false;
            CheckIfSimIsComplete();
        }
    }

    //Check if the sim is finished and set progress to 100%.
    public void CheckIfSimIsComplete()
    {
        Debug.Log("<color=magenta>[TC_FeetInPlace.cs]</color> Checking if Sim is complete...");

        if (Col_LeftBackArmPlaceDefault.GetComponent<TC_FeetReturned>().IsFootInCollision &&
            Col_LeftFrontArmPlaceDefault.GetComponent<TC_FeetReturned>().IsFootInCollision &&
            Col_RightBackArmPlaceDefault.GetComponent<TC_FeetReturned>().IsFootInCollision &&
            Col_RightFrontArmPlaceDefault.GetComponent<TC_FeetReturned>().IsFootInCollision)
        {
            Debug.Log("<b><color=magenta>[TC_FeetInPlace.cs]</color> <color=#5DF958>Simulation complete!</color></b>");
            m_photonView.RPC("SetTo100", RpcTarget.AllBuffered);
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

    private void TurnOffSecondCollisions()
    {
        Col_LeftBackArmPlaceDefault.SetActive(false);
        Col_LeftFrontArmPlaceDefault.SetActive(false);
        Col_RightBackArmPlaceDefault.SetActive(false);
        Col_RightFrontArmPlaceDefault.SetActive(false);
    }
}
