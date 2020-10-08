using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class WrenchManager : MonoBehaviour, Photon.Pun.IPunObservable
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
    private bool IsThereABitInSocket;
    public bool TheBitIsCorrect = false;

    // Update is called once per frame
    void Update()
    {
        if (DoesBitNeedCheck == true)
        {
            CheckForBit();

            if (BitSocket.GetComponent<XRSocketInteractor>().selectTarget.gameObject == null)
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

    public void CheckForBit()
    {
        if (BitSocket.GetComponent<XRSocketInteractor>().selectTarget.gameObject != null)
        {
            BitInSocket = BitSocket.GetComponent<XRSocketInteractor>().selectTarget.gameObject;
        }
    }

    public void IsBitCorrect()
    {
        if (IsThereABitInSocket == true)
        {
            if (BitInSocket == CorrectBit && TheBitIsCorrect == false)
            {
                //Great success. Unscrew the thing or send another bool to allow it to happen with collision.
                CorrectBit.GetComponent<XRSocketInteractor>().enabled = true;
                DoesBitNeedCheck = false;
                Debug.Log("<color=yellow>[WrenchManager.cs] </color> Correct bit is in socket.");
                TheBitIsCorrect = true;
            }
            else
            {
                DoesBitNeedCheck = true;
                TheBitIsCorrect = false;
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
            stream.SendNext(BitInSocket);
            stream.SendNext(TheBitIsCorrect);
        }
        else //The remote client uses this.
        {
            //This should get the data from the network.
            //this.VARIABLE = (VARIABLE TYPE)stream.RecieveNext();
            this.IsThereABitInSocket = (bool)stream.ReceiveNext();
            this.BitInSocket = (GameObject)stream.ReceiveNext();
            this.TheBitIsCorrect = (bool)stream.ReceiveNext();
        }
    }
}
