using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class WrenchManager : MonoBehaviourPunCallbacks, Photon.Pun.IPunObservable
{
    [Header("Object References")]
    public GameObject PneumaticWrench;
    public GameObject BitSocket;
    [Space(10)]
    public GameObject Bit10mm;
    public GameObject Bit12mm;
    public GameObject Bit14mm;
    public GameObject Bit16mm;
    public GameObject Bit18mm;
    public GameObject Bit20mm;
    public GameObject BitExtender;

    [Header("Parameters")]
    public GameObject CorrectBit;

    private GameObject BitInSocket;
    private bool DoesBitNeedCheck = true;
    private bool IsThereABitInSocket = false;
    public bool TheBitIsCorrect = false;

    [Header("UI")]
    public GameObject UI_TaskComplete_CorrectBit;
    public PhotonView m_photonView;

    private XRSocketInteractor BitXRSocket;
    private ProgressChecker ProgressChecker;

    private void Start()
    {
        BitXRSocket = BitSocket.GetComponent<XRSocketInteractor>();
        ProgressChecker = FindObjectOfType<ProgressChecker>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check to see if a check is neccessary.
        if (DoesBitNeedCheck == true)
        {
            CheckForBit();

            if (BitXRSocket.selectTarget.gameObject == null)
            {
                IsThereABitInSocket = false;
            }
            else
            {
                IsThereABitInSocket = true;
                IsBitCorrect();
            }
        }
    }

    //Check to see if there is a bit present in the wrench.
    public void CheckForBit()
    {
        if (BitXRSocket.selectTarget.gameObject != null)
        {
            BitInSocket = BitXRSocket.selectTarget.gameObject;
        }
        else
        {
            return;
        }
    }

    //Check is the bit is correct.
    public void IsBitCorrect()
    {
        //Checks if a bit is present in the wrench...
        if (IsThereABitInSocket == true)
        {
            //If the socket contains the right bit...
            if (BitInSocket == CorrectBit && TheBitIsCorrect == false) //TheBitIsCorrect checks if it has been stated to be correct by this system; allows 1 time use.
            {
                //Great success. Unscrew the thing or send another bool to allow it to happen with collision.
                CorrectBit.GetComponent<XRSocketInteractor>().enabled = true;
                DoesBitNeedCheck = false;
                Debug.Log("<color=yellow>[WrenchManager.cs] </color> Correct bit is in socket.");
                TheBitIsCorrect = true;
                m_photonView.RPC("UpdatePercentageUp", RpcTarget.AllBuffered); //Photon.
                UI_TaskComplete_CorrectBit.SetActive(true);
            }
            else
            {
                DoesBitNeedCheck = true;
                TheBitIsCorrect = false;
                UI_TaskComplete_CorrectBit.SetActive(false);
            }
        }
    }

    //This void allows the object to be synced using Photon View.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //The recieve order MUST be the send as the same order.
        if (stream.IsWriting) //The Local Client uses this.
        {
            //stream.SendNext(VARIABLE TO SYNC);
            stream.SendNext(IsThereABitInSocket);
            //stream.SendNext(BitInSocket);
            stream.SendNext(TheBitIsCorrect);
        }
        else //The remote client uses this.
        {
            //This should get the data from the network.
            //this.VARIABLE = (VARIABLE TYPE)stream.RecieveNext();
            this.IsThereABitInSocket = (bool)stream.ReceiveNext();
            //this.BitInSocket = (GameObject)stream.ReceiveNext();
            this.TheBitIsCorrect = (bool)stream.ReceiveNext();
        }
    }

    //Photon progress RPCCall updates.
    [PunRPC]
    void UpdatePercentageUp()
    {
        ProgressChecker.IncreasePercentageBy(5);
    }

    [PunRPC]
    void UpdatePercentageDown()
    {
        ProgressChecker.DecreasePercentageBy(5);
    }
}
