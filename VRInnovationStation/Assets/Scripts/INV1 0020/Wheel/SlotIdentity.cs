using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SlotIdentity : MonoBehaviour
{
    [Header("Identity")]
    public int SlotNumber;

    [SerializeField]
    public enum Type { WheelSlot, BreakSlot };
    public Type SlotType;

    public bool BoltInSlot = true;

    public WheelManager WheelManager;

    private void Start()
    {
        GetComponent<XRSocketInteractor>().enabled = false;
    }

    private void Update()
    {
    
    }

    private void OnTriggerEnter(Collider col)
    {
        if (WheelManager.IsNewWheelAttached == true)
        {
            GetComponent<XRSocketInteractor>().enabled = true;
        }
        else
        {
            Debug.Log("Wrong object or slot is full.");
        }
    }
}
