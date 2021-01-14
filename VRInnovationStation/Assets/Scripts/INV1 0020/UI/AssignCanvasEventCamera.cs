using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignCanvasEventCamera : MonoBehaviour
{
    void Awake()
    {
        Canvas canvas = this.GetComponent<Canvas>();

        Debug.Log("Canvas: " + canvas + "; Canvas Camera: " +  canvas.worldCamera + ".");

        if (canvas.worldCamera == null)
        {
            Debug.Log("Camera not found. Initializing XR Camera...");
            canvas.worldCamera = Camera.main;
        }
    }

    public void AttachCamera()
    {
        Canvas canvas = this.GetComponent<Canvas>();

        Debug.Log("Canvas: " + canvas + "; Canvas Camera: " + canvas.worldCamera + ".");

        Debug.Log("Camera not found. Initializing XR Camera...");
        canvas.worldCamera = Camera.main;
        Debug.Log("Camera initialized as <" + Camera.main.name + ">.");
    }
}
