using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class InputListenerSecondaryButton : MonoBehaviour
{
    List<InputDevice> devices;
    public XRNode controllerNode;
    public GameObject rayInteractorController;

    [Tooltip("Event when the button starts being pressed")]
    public UnityEvent OnPress;

    [Tooltip("Event when the button starts being released")]
    public UnityEvent OnRelease;

    //keep track of whether we are pressing the button
    bool isPressed = false;

    private void Awake()
    {
        devices = new List<InputDevice>();
    }

    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(controllerNode, devices);
       
    }

    // Start is called before the first frame update
    void Start()
    {
        GetDevice();
    }

    // Update is called once per frame
    void Update()
    {
        GetDevice();
        foreach (var device in devices)
        {
            //Debug.Log(device.name + " " + device.characteristics);

            if (device.isValid)
            {
                bool inputValue;

                if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out inputValue) && inputValue)
                {
                    if (!isPressed)
                    {
                        isPressed = true;
                        Debug.Log("OnPress event is called");

                        OnPress.Invoke();
                    }

                }
                else if (isPressed)
                {
                    isPressed = false;
                    OnRelease.Invoke();
                    Debug.Log("OnRelease event is called");

                    rayInteractorController.GetComponent<XRRayInteractor>().attachTransform.gameObject.GetComponent<Rigidbody>().useGravity = true;

                }
            }
            
        }
    }
}
