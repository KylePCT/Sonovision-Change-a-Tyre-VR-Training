using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class SlotIdentity : MonoBehaviour
{
    [Header("Identity")]
    public int SlotNumber;

    [SerializeField]
    public enum Type { WheelSlot, BreakSlot };
    public Type SlotType;

    public bool BoltInSlot = true;

    public WheelManager WheelManager;
    public WrenchManager WrenchManager;

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
            if (WheelManager.CanNewWheelBeAttached == true)
            {
                if (SlotType == 0)
                {
                    //Slots for bolts are now active on the new wheel.
                    GetComponent<XRSocketInteractor>().enabled = true;
                }
            }
            else
            {
                WrenchManager.CorrectBit.GetComponent<XRSocketInteractor>().enabled = true;
                Debug.Log("<color=orange>[SlotIdentity.cs]</color> New wheel is not yet attached.");
            }
        }
    }
}
