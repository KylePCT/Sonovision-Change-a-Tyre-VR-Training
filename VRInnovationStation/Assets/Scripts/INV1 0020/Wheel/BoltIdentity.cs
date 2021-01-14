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
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    private void Update()
    {
        if (WrenchManager.TheBitIsCorrect == true)
        {
            this.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WrenchBit" && WrenchManager.TheBitIsCorrect == true)
        {
            if (WrenchManager.CorrectBit.GetComponent<XRSocketInteractor>().selectTarget == null)
            {
                InSlot = false;
                this.gameObject.transform.SetParent(null);
                WheelManager.AreAllBoltsRemoved();
                Debug.Log("<color=orange>[BoltIdentity.cs]</color> Bolt: " + gameObject.name + " is now removed from the old wheel.");

                FindObjectOfType<AudioManager>().PlaySound("PneumaticWrench");
            }
        }
    }
}

