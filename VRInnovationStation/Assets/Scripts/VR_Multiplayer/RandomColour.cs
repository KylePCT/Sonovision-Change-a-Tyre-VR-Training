using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RandomColour : MonoBehaviourPunCallbacks, IPunObservable
{
    private Color randomColour;

    public GameObject head;
    public GameObject body;
    public GameObject leftHand;
    public GameObject rightHand;

    void Start()
    {
        randomColour = Random.ColorHSV(0f, 1f, 0.6f, .8f, 1f, 1f);
        changeColour();
        photonView.RPC("changeColour", RpcTarget.AllBuffered, randomColour);
    }

    [PunRPC]
    public void changeColour()
    {
        head.GetComponent<Renderer>().material.color = randomColour;
        body.GetComponent<Renderer>().material.color = randomColour;
        leftHand.GetComponentInChildren<Renderer>().material.color = randomColour;
        rightHand.GetComponentInChildren<Renderer>().material.color = randomColour;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //The recieve order MUST be the send as the same order.
        if (stream.IsWriting) //The Local Client uses this.
        {
            //stream.SendNext(VARIABLE TO SYNC);
            stream.SendNext(randomColour);
        }
        else //The remote client uses this.
        {
            //This should get the data from the network.
            //this.VARIABLE = (VARIABLE TYPE)stream.RecieveNext();
            this.randomColour = (Color32)stream.ReceiveNext();
        }
    }
}
