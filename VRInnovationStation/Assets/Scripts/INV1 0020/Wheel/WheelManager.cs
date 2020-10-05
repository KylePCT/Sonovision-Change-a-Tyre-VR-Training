using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WheelManager : MonoBehaviour
{
    [Header("Script References")]
    public WrenchManager WrenchManager;

    [Header("Object References")]
    public GameObject WheelMain;
    public GameObject WheelBreakDisk;
    public GameObject WheelBoltsManager;

    [Header("Bolts")]
    public GameObject[] Bolts;
    public GameObject[] WheelMainBoltHoles;
    public GameObject[] WheelBreakBoltHoles;

    [HideInInspector]
    public bool IsNewWheelAttached = false;

    // Start is called before the first frame update
    void Start()
    {
        Bolts = GameObject.FindGameObjectsWithTag("Bolts");
        WheelMainBoltHoles = GameObject.FindGameObjectsWithTag("WheelBoltHoles");
        WheelBreakBoltHoles = GameObject.FindGameObjectsWithTag("BreakBoltHoles");

        SortBoltArrays();

        IsNewWheelAttached = false;
        WheelMain.GetComponent<BoxCollider>().enabled = false;
    }

    public void AreAllBoltsRemoved()
    {
        for (int i = 0; i < Bolts.Length; ++i)
        {
            if (Bolts[i].GetComponent<SlotIdentity>().BoltInSlot == true)
            {
                return;
            }
        }

        WheelMain.GetComponent<BoxCollider>().enabled = true;
        WheelMain.GetComponent<Rigidbody>().useGravity = true;
        WheelMain.layer = 11;
        IsNewWheelAttached = true;
        //You can remove the wheel!
    }

    public void DoAllSlotsHaveBolts()
    {
        for (int i = 0; i < Bolts.Length; ++i)
        {
            if (Bolts[i].GetComponent<SlotIdentity>().BoltInSlot == false)
            {
                //Check for nothing.
            }
        }

        //All bolts are in the new wheel.
    }

    public void SortBoltArrays()
    {
        Bolts = Bolts.OrderBy(c => c.name).ToArray();
        WheelMainBoltHoles = WheelMainBoltHoles.OrderBy(c => c.name).ToArray();
        WheelBreakBoltHoles = WheelBreakBoltHoles.OrderBy(c => c.name).ToArray();
    }
}
