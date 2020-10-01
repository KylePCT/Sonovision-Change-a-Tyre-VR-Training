using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignCanvasEventCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas && canvas.worldCamera == null)
        {
            Debug.Log("Camera not found. Initializing XR Camera...");
            canvas.worldCamera = Camera.main;
            Debug.Log("Camera initialized as <" + Camera.main + ">.");
        }
    }
}
