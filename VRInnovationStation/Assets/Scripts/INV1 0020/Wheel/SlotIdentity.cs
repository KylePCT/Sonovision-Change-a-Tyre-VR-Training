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

    private ProgressChecker ProgressChecker;

    private void Start()
    {
        ProgressChecker = FindObjectOfType<ProgressChecker>();

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
            if (WheelManager.CanNewWheelBeAttached == true && SlotType == 0 && !WheelManager.IsNewWheelAttached)
            {
                //Slots for bolts are now active on the new wheel.
                GetComponent<XRSocketInteractor>().enabled = true;

                //Change bolt values (the colliding bolt 'col').
                col.GetComponent<BoltIdentity>().InSlot = true; //Tell the bolt it is now in a slot.
                col.GetComponent<BoltIdentity>().needsTightening = true;

                m_photonView.RPC("IncreaseProgress", RpcTarget.AllBuffered); //Photon for percentage sets.
                Debug.Log("<color=orange>[SlotIdentity.cs]</color> Bolt <" + col.gameObject.name + "> is now in a slot.");

                WheelManager.DoAllSlotsHaveBolts();
            }
            else
            {
                WrenchManager.CorrectBit.GetComponent<XRSocketInteractor>().enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Bolts")
        {
            if (WheelManager.CanNewWheelBeAttached == true && SlotType == 0)
            {
                col.GetComponent<BoltIdentity>().InSlot = false;
                m_photonView.RPC("DecreaseProgress", RpcTarget.AllBuffered); //Photon for percentage sets.
            }
        }
    }

    [PunRPC]
    void IncreaseProgress()
    {
        ProgressChecker.IncreasePercentageBy(2);
    }

    [PunRPC]
    void DecreaseProgress()
    {
        ProgressChecker.DecreasePercentageBy(2);
    }
}
