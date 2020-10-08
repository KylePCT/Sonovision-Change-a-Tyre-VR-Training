﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RandomColour : MonoBehaviourPunCallbacks
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
}
