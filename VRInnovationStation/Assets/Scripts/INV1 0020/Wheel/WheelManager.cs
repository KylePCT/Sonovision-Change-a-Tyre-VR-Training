using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class WheelManager : MonoBehaviourPunCallbacks
{
    [Header("Script References")]
    public WrenchManager WrenchManager;
    public SnapNewWheel WheelSnap;
    public PhotonView m_photonView;

    [Header("Object References")]
    public GameObject WheelMain;
    public GameObject WheelBreakDisk;
    public GameObject WheelBoltsManager;
    public GameObject NewWheel;

    [Header("Bolts")]
    public GameObject[] Bolts;
    public GameObject[] WheelMainBoltHoles;
    public GameObject[] WheelBreakBoltHoles;

    [HideInInspector]
    public bool CanNewWheelBeAttached = false;
    [HideInInspector]
    public bool IsNewWheelAttached = false;

    public GameObject[] ChassisCollisions;
    public GameObject[] OriginCollisions;

    //One time checks.
    private bool wheelHasBeenRemoved = false;
    private bool wheelHasBeenReplaced = false;

    // Start is called before the first frame update
    void Start()
    {
        Bolts = GameObject.FindGameObjectsWithTag("Bolts");
        WheelMainBoltHoles = GameObject.FindGameObjectsWithTag("WheelBoltHoles");
        WheelBreakBoltHoles = GameObject.FindGameObjectsWithTag("BreakBoltHoles");
        WheelMain.GetComponent<MeshCollider>().enabled = false;
        IsNewWheelAttached = false;

        SortBoltArrays();

        CanNewWheelBeAttached = false;

        wheelHasBeenRemoved = false;
        wheelHasBeenReplaced = false;
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

        //Wheel can now be removed. Allow the next stuff.
        WheelMain.GetComponent<MeshCollider>().enabled = true;
        WheelMain.layer = 11; //Make it an interactable.

        NewWheel.GetComponent<MeshCollider>().enabled = true;
        NewWheel.GetComponent<Rigidbody>().isKinematic = true;

        CanNewWheelBeAttached = true;
        WheelSnap.CanSnap = true;
        Debug.Log("<color=orange>[WheelManager.cs]</color> Wheel can now be removed.");
        m_photonView.RPC("WheelRemovedTask", RpcTarget.AllBuffered); //Photon for percentage sets.
    }

    //Checks if the slots all have bolts. This would be called after you have put the new wheel on.
    public void DoAllSlotsHaveBolts()
    {
        if (CanNewWheelBeAttached)
        {
            Debug.Log("<color=orange>[WheelManager.cs]</color> Checking bolts in slots...");

            for (int i = 0; i < Bolts.Length; i++)
            {
                Debug.Log("<color=orange>[WheelManager.cs]</color> Checking bolt <" + i + ">.");

                //If any of the bolts in the array are not in slot, call return.
                if (Bolts[i].GetComponent<BoltIdentity>().InSlot == false)
                {
                    Debug.Log("<color=orange>[WheelManager.cs]</color> Task not completed, one or more bolts are still needed.");
                    return;
                    //Check for nothing.
                }

                Bolts[i].GetComponent<BoltIdentity>().needsTightening = true;
            }

            IsNewWheelAttached = true;
            Debug.Log("<color=orange>[WheelManager.cs]</color> Wheel has all bolts.");
        }
    }

    public void AreAllBoltsTight()
    {
        for (int i = 0; i < Bolts.Length; i++)
        {
            Debug.Log("<color=orange>[WheelManager.cs]</color> Checking tightness for bolt <" + i + ">.");

            //If any of the bolts in the array are not tight, call return.
            if (Bolts[i].GetComponent<BoltIdentity>().IsTight == false)
            {
                Debug.Log("<color=orange>[WheelManager.cs]</color> Task not completed, one or more bolts are still not tight.");
                return;
                //Check for nothing.
            }
        }

        //Change the collisions around.
        foreach (GameObject i in ChassisCollisions)
        {
            i.SetActive(false);
        }

        foreach (GameObject j in OriginCollisions)
        {
            j.SetActive(true);
        }

        Debug.Log("<color=orange>[WheelManager.cs]</color> Arm collisions are now inverted.");
        FindObjectOfType<AudioManager>().PlaySound("UI_Complete");

        m_photonView.RPC("WheelReplacedTask", RpcTarget.AllBuffered); //Photon for percentage sets.
    }

    //Sorts the arrays into descending order for convinience.
    public void SortBoltArrays()
    {
        Bolts = Bolts.OrderBy(c => c.name).ToArray();
        WheelMainBoltHoles = WheelMainBoltHoles.OrderBy(c => c.name).ToArray();
        WheelBreakBoltHoles = WheelBreakBoltHoles.OrderBy(c => c.name).ToArray();
    }

    //PunRPC calls for percentage changes.
    [PunRPC]
    void WheelRemovedTask()
    {
        if (!wheelHasBeenRemoved)
        {
            WheelMain.GetComponent<MeshCollider>().enabled = true;
            FindObjectOfType<ProgressChecker>().ChangePercentageTo(55);
            WrenchManager.CorrectBit.GetComponent<XRSocketInteractor>().enabled = false;
            wheelHasBeenRemoved = true;
        }
    }

    [PunRPC]
    void WheelReplacedTask()
    {
        if (!wheelHasBeenReplaced)
        {
            FindObjectOfType<ProgressChecker>().ChangePercentageTo(90);
            wheelHasBeenReplaced = true;
        }
    }
}
