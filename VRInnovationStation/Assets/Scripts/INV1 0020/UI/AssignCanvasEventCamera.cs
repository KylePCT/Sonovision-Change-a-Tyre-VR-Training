using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignCanvasEventCamera : MonoBehaviour
{
    void Awake()
    {
        //Find the canvas.
        Canvas canvas = this.GetComponent<Canvas>();

        Debug.Log("<color=#F392CA>[AssignCanvasEventCamera.cs]</color> Initializing Camera for Canvas: <" + canvas + ">; Canvas Camera: <" +  canvas.worldCamera + ">.");

        //If the canvas has no camera, find it.
        if (canvas.worldCamera == null)
        {
            Debug.Log("<color=#F392CA>[AssignCanvasEventCamera.cs]</color> Camera not found. Initializing XR Camera...");
            canvas.worldCamera = Camera.main;
        }
    }

    //Attach the main camera as the world Camera.
    public void AttachCamera()
    {
        Canvas canvas = this.GetComponent<Canvas>();

        Debug.Log("<color=#F392CA>[AssignCanvasEventCamera.cs]</color> Initializing Camera for Canvas: <" + canvas + ">; Canvas Camera: <" + canvas.worldCamera + ">.");

        Debug.Log("<color=#F392CA>[AssignCanvasEventCamera.cs]</color> Camera not found. Initializing XR Camera...");
        canvas.worldCamera = Camera.main;
        Debug.Log("<color=#F392CA>[AssignCanvasEventCamera.cs]</color> Camera initialized as <" + Camera.main.name + ">.");
    }
}
