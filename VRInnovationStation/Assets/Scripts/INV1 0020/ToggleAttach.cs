using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ToggleAttach : MonoBehaviour
{
    public GameObject Transform_A;
    public GameObject Transform_B;

    private XRGrabInteractable Grab;

    private void Start()
    {
        Grab = this.GetComponent<XRGrabInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Left Base Controller")
        {
            AttachLeft();
        }
        else if (other.gameObject.name == "Right Base Controller Direct")
        {
            AttachRight();
        }
    }

    public void AttachLeft()
    {
        Grab.attachTransform = Transform_A.transform;
    }

    public void AttachRight()
    {
        Grab.attachTransform = Transform_B.transform;
    }
}
