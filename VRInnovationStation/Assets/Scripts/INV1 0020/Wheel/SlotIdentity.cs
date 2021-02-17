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

    private GameObject AttachedBolt;

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
        //Failsafe, make sure the wrench has an active socket interactor unless otherwise.
        if (WrenchManager.CorrectBit.GetComponent<XRSocketInteractor>().enabled == false)
        {
            WrenchManager.CorrectBit.GetComponent<XRSocketInteractor>().enabled = true;
        }

        //If the slot is in contact with a bolt, allow it to attach to the socket.
        if (col.tag == "Bolts")
        {
            AttachedBolt = col.gameObject;

            if (WheelManager.CanNewWheelBeAttached == true && SlotType == 0)
            {
                if (WrenchManager.CorrectBit.GetComponent<XRSocketInteractor>().selectTarget != null)
                {
                    //Slots for bolts are now active on the new wheel.
                    GetComponent<XRSocketInteractor>().enabled = true;
                    
                    m_photonView.RPC("IncreaseProgress", RpcTarget.AllBuffered); //Photon for percentage sets.
                    AttachedBolt.GetComponent<BoltIdentity>().InSlot = true; //Tell the bolt it is now in a slot.
                    BoltInSlot = false;

                    Debug.Log("<color=orange>[SlotIdentity.cs]</color> Bolt <" + col.gameObject.name + "> is now in a slot.");
                    Debug.Log("<color=orange>[SlotIdentity.cs]</color> Socket currently has: <" + WrenchManager.CorrectBit.GetComponent<XRSocketInteractor>().selectTarget.name + "> attached.");

                    WheelManager.DoAllSlotsHaveBolts();
                }
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
