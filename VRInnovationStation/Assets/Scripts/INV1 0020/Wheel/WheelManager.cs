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
    public GameObject UI_Canvas_2k;
    public GameObject UI_Canvas_2o;
    public GameObject CorrectBit;
    [Space(10)]
    public Material Bolt_StandardMat;
    public Material Bolt_HighlightMat;
    public Material Bit_StandardMat;
    public Material Bit_HighlightMat;

    [Space(10)]
    [Header("Object References")]
    public GameObject WheelMain;
    public GameObject WheelBreakDisk;
    public GameObject WheelBoltsManager;
    public GameObject NewWheel;

    [Header("Bolts")]
    public GameObject[] Bolts;
    public GameObject[] WheelMainBoltHoles;
    public GameObject[] WheelBreakBoltHoles;

    [Space(10)]
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

        //Checks all bolts; if one is in slot, return.
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

        //If code is allowed to execute...
        WheelMain.GetComponent<MeshCollider>().enabled = true;
        WheelMain.layer = 11; //Make it an interactable.

        NewWheel.GetComponent<MeshCollider>().enabled = true;
        NewWheel.GetComponent<Rigidbody>().isKinematic = true;

        CanNewWheelBeAttached = true;
        WheelSnap.CanSnap = true;

        UI_Canvas_2k.SetActive(true);
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
                Bolts[i].gameObject.layer = 1;

                //If any of the bolts in the array are not in slot, call return.
                if (Bolts[i].GetComponent<BoltIdentity>().InSlot == false)
                {
                    Debug.Log("<color=orange>[WheelManager.cs]</color> Task not completed, one or more bolts are still needed.");
                    return;
                    //Check for nothing.
                }

                SetBoltHighlights();
            }

            //If all bolts are in the slots on the wheel...
            CorrectBit.GetComponent<Renderer>().material = Bit_HighlightMat;
            IsNewWheelAttached = true;
            Debug.Log("<color=orange>[WheelManager.cs]</color> Wheel has all bolts.");
        }
    }

    public void AreAllBoltsTight()
    {
        CorrectBit.GetComponent<Renderer>().material = Bit_HighlightMat; //Show the user what to do.

        //Check is all bolts are tight...
        for (int i = 0; i < Bolts.Length; i++)
        {
            Debug.Log("<color=orange>[WheelManager.cs]</color> Checking tightness for bolt <" + i + ">.");

            //If any of the bolts in the array are not tight, call return.
            if (Bolts[i].GetComponent<BoltIdentity>().IsTight == false)
            {
                Debug.Log("<color=orange>[WheelManager.cs]</color> Task not completed, one or more bolts are still not tight.");
                Bolts[i].GetComponent<Renderer>().material = Bolt_HighlightMat;
                return;
                //Check for nothing.
            }

            Bolts[i].GetComponent<Renderer>().material = Bolt_StandardMat;
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

        UI_Canvas_2o.SetActive(true);

        //Remove functionality from the wrench bit.
        CorrectBit.GetComponent<Renderer>().material = Bit_StandardMat;
        CorrectBit.GetComponent<XRSocketInteractor>().enabled = false;

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

    public void SetBoltHighlights()
    {
        StartCoroutine(SetBoltHighlightsCoroutine(1));
    }

    IEnumerator SetBoltHighlightsCoroutine(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        for (int i = 0; i < Bolts.Length; i++)
        {
            //Only show the highlighted material if it isn't already tight.
            if (!Bolts[i].GetComponent<BoltIdentity>().IsTight && Bolts[i].GetComponent<BoltIdentity>().InSlot)
            {
                Bolts[i].GetComponent<Renderer>().material = Bolt_HighlightMat;
                Bolts[i].GetComponent<BoltIdentity>().needsTightening = true;

                //Remove the bolts interactions so the material doesn't change when hovered.
                Bolts[i].GetComponent<XRGrabInteractable>().interactionLayerMask = 0;
                Bolts[i].GetComponent<XRGrabInteractable>().colliders.Clear();
                Bolts[i].gameObject.layer = 0;
                Debug.Log("<color=orange>[WheelManager.cs]</color> Bolt <" + Bolts[i].gameObject.name + "> is now highlighted and has had its interactions removed.");
            }
        }
    }

    //PunRPC calls for percentage changes.
    [PunRPC]
    void WheelRemovedTask()
    {
        if (!wheelHasBeenRemoved)
        {
            WheelMain.GetComponent<MeshCollider>().enabled = true;
            FindObjectOfType<ProgressChecker>().ChangePercentageTo(55);
            wheelHasBeenRemoved = true;
        }
    }

    [PunRPC]
    void WheelReplacedTask()
    {
        if (!wheelHasBeenReplaced)
        {
            CorrectBit.GetComponent<XRSocketInteractor>().enabled = false;
            NewWheel.gameObject.layer = 0;
            FindObjectOfType<ProgressChecker>().ChangePercentageTo(90);
            wheelHasBeenReplaced = true;
        }
    }
}
