using System.Collections;
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

    public WheelManager WheelManager;

    private void Start()
    {
        //Remove collider to prevent movement.
        NewWheel.GetComponent<XRGrabInteractable>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the collider is triggered by 'Wheel_New', and other scripts have set their completed values...
        if (other.name == "Wheel_New" && WheelManager.CanNewWheelBeAttached && CanSnap)
        {
            SnapWheel();
            NewWheel.GetComponent<XRGrabInteractable>().enabled = false;
        }
    }
    
    //Destroy old components and set the new whwel to have these.
    public void SnapWheel()
    {
        NewWheel.GetComponent<XRGrabInteractable>().enabled = false;
        OldWheel.gameObject.transform.SetParent(null);

        NewWheel.gameObject.transform.SetParent(SnapParent.gameObject.transform);
        NewWheel.gameObject.transform.position = AttachPoint.position;
        NewWheel.gameObject.transform.rotation = AttachPoint.rotation;

        Debug.Log("<color=orange>[SnapNewWheel.cs]</color> New wheel has been added to the chassis.");
    }
}
