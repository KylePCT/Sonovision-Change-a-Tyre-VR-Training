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
    public GameObject[] ChassisCollisions;

    //The origin collisions.
    [Header("Origin Collisions")]
    public GameObject[] OriginCollisions;

    //UI and Script References.
    [Header("References")]
    public GameObject UI_ProgressTask_2e;
    public GameObject UI_ProgressTask_2v;
    public WheelManager whManager;

    private bool UI_ProgressTaskComplete = false;
    public bool AreAllFeetInPlace = false;
    public bool CarLiftCanMove = false;
    private bool has100BeenSet = false;

    private AudioManager AudioManager;
    private ProgressChecker ProgressChecker;

    private void Start()
    {
        //Turns off the guides.
        DeactivateAllMeshRenderers();
        TurnOffSecondCollisions();

        UI_ProgressTaskComplete = false;

        AudioManager = FindObjectOfType<AudioManager>();
        ProgressChecker = FindObjectOfType<ProgressChecker>();
    }

    //Check if the feet are in the collision to allow the lift to be raised correctly.
    void Update()
    {
        for (int i = 0; i < ChassisCollisions.Length; i++)
        {
            if (ChassisCollisions[i].GetComponent<TC_FeetInPlace_Single>().IsFootInCollision == false)
            {
                Debug.Log("<color=magenta>[TC_FeetInPlace.cs]</color> All four feet are not in place. Returning...");
                AreAllFeetInPlace = false;
                CheckIfSimIsComplete();
                return;
            }

            //Set variables to true and run multiplayer events.
            AreAllFeetInPlace = true;
            CarLiftCanMove = true;
            m_photonView.RPC("SetActiveUIElements", RpcTarget.AllBuffered);
            DeactivateAllMeshRenderers();
            Debug.Log("<color=magenta>[TC_FeetInPlace.cs]</color> Lift can now be raised. All four feet are in place.");
        }
    }

    //Check if the sim is finished and set progress to 100%.
    public void CheckIfSimIsComplete()
    {
        Debug.Log("<color=magenta>[TC_FeetInPlace.cs]</color> Checking if Sim is complete...");

        for (int i = 0; i < OriginCollisions.Length; i++)
        {
            if (OriginCollisions[i].GetComponent<TC_FeetReturned>().IsFootInCollision == false)
            {
                Debug.Log("<color=magenta>[TC_FeetInPlace.cs]</color> All four feet are back at the origin. Returning...");
                return;
            }
        }

        Debug.Log("<b><color=magenta>[TC_FeetInPlace.cs]</color> <color=#5DF958>Simulation complete!</color></b>");

        if (!has100BeenSet)
        {
            FindObjectOfType<ProgressChecker>().ChangePercentageTo(100);
            m_photonView.RPC("SetTo100", RpcTarget.AllBuffered);
            has100BeenSet = true;
        }

    }

    //Set the UI to be updated for all players. Photon Multiplayer.
    [PunRPC]
    void SetActiveUIElements()
    {
        if (UI_ProgressTaskComplete == false)
        {
            UI_ProgressTask_2e.SetActive(true);
            AudioManager.PlaySound("UI_Complete");
            ProgressChecker.ChangePercentageTo(25);
            UI_ProgressTaskComplete = true;
        }
    }

    //Update progress to 100% for all players. Photon Multiplayer.
    [PunRPC]
    void SetTo100()
    {
        if (!has100BeenSet)
        {
            UI_ProgressTask_2v.SetActive(true);
            AudioManager.PlaySound("UI_Complete");
            ProgressChecker.ChangePercentageTo(100);
            has100BeenSet = true;
        }
    }

    //Hide all guides; in its own method for button calls and ease of use.
    public void DeactivateAllMeshRenderers()
    {
        for (int i = 0; i < ChassisCollisions.Length; i++)
        {
            ChassisCollisions[i].GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void TurnOffSecondCollisions()
    {
        for (int i = 0; i < OriginCollisions.Length; i++)
        {
            OriginCollisions[i].gameObject.SetActive(false);
        }
    }
}
