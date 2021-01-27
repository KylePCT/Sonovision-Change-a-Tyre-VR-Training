using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class SlotIdentity : MonoBehaviourPunCallbacks
{
    [Header("Identity")]
    public int SlotNumber;

    [SerializeField]
    public enum Type { WheelSlot, BreakSlot };
    public Type SlotType;

    public bool BoltInSlot = true;

    public WheelManager WheelManager;
    public WrenchManager WrenchManager;
    public PhotonView m_photonView;

    private void Start()
    {
        //If the slot is on the wheel, set the socket to inactive.
        if (SlotType == 0)
        {
            GetComponent<XRSocketInteractor>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        //If the slot is in contact with a bolt, allow it to attach to the socket.
        if (col.tag == "Bolts")
        {
            if (WheelManager.CanNewWheelBeAttached == true && SlotType == 0)
            {
                //Slots for bolts are now active on the new wheel.
                GetComponent<XRSocketInteractor>().enabled = true;
                m_photonView.RPC("IncreaseProgress", RpcTarget.AllBuffered); //Photon for percentage sets.
                WheelManager.DoAllSlotsHaveBolts();
            }
            else
            {
                WrenchManager.CorrectBit.GetComponent<XRSocketInteractor>().enabled = true;
                Debug.Log("<color=orange>[SlotIdentity.cs]</color> Bolt not attached to slot. CanNewWheelBeAttached is false or SlotType is not a wheel slot.");
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        //If the slot is in contact with a bolt, allow it to attach to the socket.
        if (col.tag == "Bolts")
        {
            if (WheelManager.CanNewWheelBeAttached == true && SlotType == 0)
            {
                m_photonView.RPC("DecreaseProgress", RpcTarget.AllBuffered); //Photon for percentage sets.
            }
        }
    }

    [PunRPC]
    void IncreaseProgress()
    {
        FindObjectOfType<ProgressChecker>().IncreasePercentageBy(2);
    }

    [PunRPC]
    void DecreaseProgress()
    {
        FindObjectOfType<ProgressChecker>().DecreasePercentageBy(2);
    }
}
