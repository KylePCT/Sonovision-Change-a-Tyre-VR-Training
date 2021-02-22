using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputListener : MonoBehaviour
{
    List<InputDevice> inputDevices;

    InputDeviceCharacteristics deviceCharacteristics;
    public XRNode controllerNode;

    private InputDevice inputDevice;
    private Animator handAnimator_L;
    private Animator handAnimator_R;

    private void Awake()
    {
        inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(deviceCharacteristics, inputDevices);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        deviceCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right;

        InputDevices.GetDevicesWithCharacteristics(deviceCharacteristics, inputDevices);

        foreach (InputDevice inputDevice in inputDevices)
        {
            Debug.Log("Device found with name :" + inputDevice.name);

            bool inputValue;
            if (inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out inputValue) && inputValue)
            {

            }
        }
    }
}
