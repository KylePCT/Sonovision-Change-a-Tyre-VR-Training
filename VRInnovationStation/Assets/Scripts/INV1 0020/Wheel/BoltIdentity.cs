using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class BoltIdentity : MonoBehaviour
{
    [Header("Identity")]
    public int BoltNumber;
    public bool InSlot;

    public WrenchManager WrenchManager;
    public WheelManager WheelManager;

    private void Start()
    {
        InSlot = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WrenchBit" && WrenchManager.TheBitIsCorrect == true)
        {
            if (WrenchManager.CorrectBit.GetComponent<XRSocketInteractor>().selectTarget == null)
            {
                InSlot = false;

                WheelManager.AreAllBoltsRemoved();
                Debug.Log("<color=orange>[BoltIdentity.cs]</color> Bolt: " + gameObject.name + " is now attached.");
            }
        }
    }
}

