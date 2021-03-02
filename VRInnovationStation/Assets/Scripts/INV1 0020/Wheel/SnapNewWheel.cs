﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapNewWheel : MonoBehaviour
{
    public GameObject OldWheel;
    public GameObject NewWheel;
    public Transform AttachPoint;
    public GameObject SnapParent;
    public bool CanSnap = false;

    private TC_FeetInPlace_Single[] chassisFeetInPlace;
    public TC_FeetInPlace feetInPlace;

    public WheelManager WheelManager;

    private void Start()
    {
        //Remove collider to prevent movement.
        NewWheel.GetComponent<MeshCollider>().enabled = false;

        chassisFeetInPlace = FindObjectsOfType<TC_FeetInPlace_Single>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the collider is triggered by 'Wheel_New', and other scripts have set their completed values...
        if (other.name == "Wheel_New" && WheelManager.CanNewWheelBeAttached && CanSnap)
        {
            SnapWheel();
        }
    }
    
    //Destroy old components and set the new wheel to have these.
    public void SnapWheel()
    {
        Destroy(NewWheel.GetComponent<XRGrabInteractable>());
        OldWheel.gameObject.transform.SetParent(null);
        NewWheel.gameObject.transform.SetParent(SnapParent.gameObject.transform);
        NewWheel.gameObject.transform.position = AttachPoint.position;
        NewWheel.gameObject.transform.rotation = AttachPoint.rotation;
        NewWheel.GetComponent<MeshCollider>().enabled = false;

        //Remove old collision methods so new collisions can run.
        foreach (TC_FeetInPlace_Single foot in chassisFeetInPlace)
        {
            foot.IsFootInCollision = false;
        }

        feetInPlace.CarLiftCanMove = true;

        Debug.Log("<color=orange>[SnapNewWheel.cs]</color> New wheel has been added to the chassis.");
    }
}
