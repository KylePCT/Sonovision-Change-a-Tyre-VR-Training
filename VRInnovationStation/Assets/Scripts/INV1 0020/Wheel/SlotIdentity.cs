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

    private void Start()
    {
        if (SlotType == 0)
        {
            GetComponent<XRSocketInteractor>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Bolts")
        {
            if (WheelManager.IsNewWheelAttached == true)
            {
                if (SlotType == 0)
                {
                    GetComponent<XRSocketInteractor>().enabled = true;
                }
            }
            else
            {
                Debug.Log("<color=orange>[SlotIdentity.cs]</color> New wheel is not yet attached.");
            }
        }
    }
}
