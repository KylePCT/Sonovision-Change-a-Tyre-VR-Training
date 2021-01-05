﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class WheelManager : MonoBehaviour
{
    [Header("Script References")]
    public WrenchManager WrenchManager;
    public SnapNewWheel WheelSnap;

    [Header("Object References")]
    public GameObject WheelMain;
    public GameObject WheelBreakDisk;
    public GameObject WheelBoltsManager;

    [Header("Bolts")]
    public GameObject[] Bolts;
    public GameObject[] WheelMainBoltHoles;
    public GameObject[] WheelBreakBoltHoles;

    [HideInInspector]
    public bool CanNewWheelBeAttached = false;
    [HideInInspector]
    public bool IsNewWheelAttached = false;

    // Start is called before the first frame update
    void Start()
    {
        Bolts = GameObject.FindGameObjectsWithTag("Bolts");
        WheelMainBoltHoles = GameObject.FindGameObjectsWithTag("WheelBoltHoles");
        WheelBreakBoltHoles = GameObject.FindGameObjectsWithTag("BreakBoltHoles");
        WheelMain.GetComponent<MeshCollider>().enabled = false;

        SortBoltArrays();

        CanNewWheelBeAttached = false;
    }

    //Checks if all the bolts are removed before allowing the user to take the wheel off.
    public void AreAllBoltsRemoved()
    {
        Debug.Log("<color=orange>[WheelManager.cs]</color> <" + Bolts.Length + "> bolts found.");

        for (int i = 0; i < Bolts.Length; i++)
        {
            if (Bolts[i].GetComponent<BoltIdentity>().InSlot == true)
            {
                Debug.Log("<color=orange>[WheelManager.cs]</color> Wheel cannot be removed yet. One or more bolts still remain.");
                CanNewWheelBeAttached = false;
                WheelSnap.CanSnap = false;
                return;
            }
        }

        WheelMain.GetComponent<MeshCollider>().enabled = true;
        WheelMain.layer = 11;
        CanNewWheelBeAttached = true;
        WheelSnap.CanSnap = true;
        Debug.Log("<color=orange>[WheelManager.cs]</color> Wheel can now be removed.");
    }

    //Checks if the slots all have bolts. This would be called after you have put the new wheel on.
    public void DoAllSlotsHaveBolts()
    {
        if (CanNewWheelBeAttached && WheelBreakDisk.GetComponent<XRSocketInteractor>().selectTarget.gameObject.name == "Wheel_New")
        {
            for (int i = 0; i < Bolts.Length; i++)
            {
                if (Bolts[i].GetComponent<BoltIdentity>().InSlot == false)
                {
                    Debug.Log("<color=orange>[WheelManager.cs]</color> Task not completed, one or more bolts are still needed.");
                    return;
                    //Check for nothing.
                }
            }

            IsNewWheelAttached = true;
            Debug.Log("<color=orange>[WheelManager.cs]</color> Wheel has all bolts and task is complete.");
        }
    }

    //Sorts the arrays into descending order for convinience.
    public void SortBoltArrays()
    {
        Bolts = Bolts.OrderBy(c => c.name).ToArray();
        WheelMainBoltHoles = WheelMainBoltHoles.OrderBy(c => c.name).ToArray();
        WheelBreakBoltHoles = WheelBreakBoltHoles.OrderBy(c => c.name).ToArray();
    }
}
