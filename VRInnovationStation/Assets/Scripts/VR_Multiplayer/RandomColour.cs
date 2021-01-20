using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RandomColour : MonoBehaviourPunCallbacks
{
    private Color randomColour;

    //Avatar components.
    public GameObject head;
    public GameObject body;
    public GameObject leftHand;
    public GameObject rightHand;

    void Start()
    {
        randomColour = Random.ColorHSV(0f, 1f, 0.6f, .8f, 1f, 1f);
        changeColour();
        //photonView.RPC("changeColour", RpcTarget.AllBuffered, randomColour);
    }

    [PunRPC]
    public void changeColour()
    {
        //If it's the player, only change the hands to the colour as the body/head will be transparent.
        if (photonView.IsMine == true)
        {
            leftHand.GetComponentInChildren<Renderer>().material.color = randomColour;
            rightHand.GetComponentInChildren<Renderer>().material.color = randomColour;

            return;
        }

        //If it's anyone else, work as intended and change all main colours.
        head.GetComponent<Renderer>().material.color = randomColour;
        body.GetComponent<Renderer>().material.color = randomColour;
        leftHand.GetComponentInChildren<Renderer>().material.color = randomColour;
        rightHand.GetComponentInChildren<Renderer>().material.color = randomColour;
    }
}
