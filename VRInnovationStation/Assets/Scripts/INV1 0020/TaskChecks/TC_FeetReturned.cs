using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

//Checks each individual foot.
public class TC_FeetReturned : MonoBehaviourPunCallbacks
{
    public bool IsFootInCollision = false;

    public WheelManager whManager;
    public PhotonView m_photonView;

    public TC_FeetInPlace feetInPlace;

    private ProgressChecker ProgressChecker;

    private void Start()
    {
        ProgressChecker = FindObjectOfType<ProgressChecker>();
    }

    //If a foot enters the collision...
    private void OnTriggerEnter(Collider other)
    {
        if (whManager.IsNewWheelAttached)
        {
            //Check tag.
            if (other.gameObject.CompareTag("Chassis_Foot"))
            {
                //Set the collision to be true and update the progress UI.
                IsFootInCollision = true;
                feetInPlace.AreAllFeetInPlace = false; //Remove old feet methods.
                feetInPlace.CheckIfSimIsComplete();
                m_photonView.RPC("UpdatePercentageUp", RpcTarget.AllBuffered);
                Debug.Log("<color=magenta>[TC_FeetReturned.cs] </color>" + gameObject.name + " is now back to it's origin.");
            }
            else
            {
                //If a random thing enters the collision, don't set variables.
                Debug.Log("<color=magenta>[TC_FeetReturned.cs] </color>" + gameObject.name + " has entered the collision and is not tagged 'Chassis_Foot'.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (whManager.IsNewWheelAttached)
        {
            //Otherwise just remove the percentage stated prior.
            if (other.gameObject.CompareTag("Chassis_Foot"))
            {
                IsFootInCollision = false;
                m_photonView.RPC("UpdatePercentageDown", RpcTarget.AllBuffered);
                Debug.Log("<color=magenta>[TC_FeetReturned.cs] </color>" + gameObject.name + " is no longer in the origin.");
            }
        }
    }

    //Progress check RPCCalls for Photon Multiplayer.
    [PunRPC]
    void UpdatePercentageUp()
    {
        ProgressChecker.IncreasePercentageBy(1);
        IsFootInCollision = true;
        feetInPlace.AreAllFeetInPlace = false; //Remove old feet methods.
        feetInPlace.CheckIfSimIsComplete();
    }

    [PunRPC]
    void UpdatePercentageDown()
    {
        ProgressChecker.DecreasePercentageBy(1);
        IsFootInCollision = false;
    }
}
