using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class BoltIdentity : MonoBehaviour
{
    [Header("Identity")]
    public int BoltNumber;
    public bool InSlot = true;

    public WrenchManager WrenchManager;
    public WheelManager WheelManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WrenchBit" && WrenchManager.TheBitIsCorrect == true)
        {
            //gameObject.transform.SetParent(WrenchManager.PneumaticWrench.transform.Find("AttachSocket"));
            Debug.Log(gameObject.name + " is now attached.");
            InSlot = false;
        }
        else
        {
            //gameObject.transform.SetParent(WheelManager.WheelBoltsManager.transform);
        }
    }
}

