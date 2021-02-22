using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Photon.Realtime;

public class BoltIdentity : MonoBehaviourPunCallbacks
{
    [Header("Identity")]
    public int BoltNumber;
    public bool InSlot = true;
    public bool needsTightening = false;
    public bool IsTight = false;

    public WrenchManager WrenchManager;
    public WheelManager WheelManager;
    public PhotonView m_photonView;

    //Bolts start in slot without colliders.
    private void Start()
    {
        InSlot = true;
        needsTightening = false;
        IsTight = false;

        this.gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    //Once the wrench has the correct bit, allow these bolts to have colliders to be interacted with. 
    private void Update()
    {
        if (WrenchManager.TheBitIsCorrect == true)
        {
            this.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }

    //On trigger...
    private void OnTriggerEnter(Collider other)
    {
        //If the correct bit is colliding...
        if (other.tag == "WrenchBit" && WrenchManager.TheBitIsCorrect == true)
        {
            //If the wrench doesn't have anything on the bit...
            if (WrenchManager.CorrectBit.GetComponent<XRSocketInteractor>().selectTarget == null)
            {
                //Remove the bolt and check if all are removed.
                InSlot = false;
                this.gameObject.transform.SetParent(null);

                WheelManager.AreAllBoltsRemoved();

                FindObjectOfType<AudioManager>().PlaySound("PneumaticWrench");
                m_photonView.RPC("UpdateProgress", RpcTarget.AllBuffered);

                Debug.Log("<color=orange>[BoltIdentity.cs]</color> Bolt: " + gameObject.name + " is now removed from the old wheel.");
            }
        }

        //If the wrench touches the bolt and the new wheel is attached.
        if (other.tag == "WrenchBit" && WheelManager.IsNewWheelAttached == true)
        {
            if (!InSlot)
            {
                Debug.LogWarning("<color=orange>[BoltIdentity.cs]</color> <color=red>Tightness check for Bolt: <" + gameObject.name + "> failed as it wasn't in a slot. </color>");
            }

            //If it still needs tightening, tighten the bolt.
            if (needsTightening == true && InSlot == true)
            {
                this.GetComponent<XRGrabInteractable>().enabled = false;
                this.GetComponent<BoxCollider>().enabled = false;
                FindObjectOfType<AudioManager>().PlaySound("PneumaticWrench");
                IsTight = true;

                UpdateProgress();
                WheelManager.AreAllBoltsTight();

                Debug.Log("<color=orange>[BoltIdentity.cs]</color> Bolt: <" + gameObject.name + "> has been tightened.");
                needsTightening = false;
            }
        }
    }

    [PunRPC]
    void UpdateProgress()
    {
        FindObjectOfType<ProgressChecker>().IncreasePercentageBy(2);
    }
}

