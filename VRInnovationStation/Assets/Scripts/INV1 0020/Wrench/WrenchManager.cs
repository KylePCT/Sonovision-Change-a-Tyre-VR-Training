using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WrenchManager : MonoBehaviour
{
    [Header("Object References")]
    public GameObject PneumaticWrench;
    public GameObject BitSocket;
    [Space(10)]
    public GameObject Bit10mm;
    public GameObject Bit12mm;
    public GameObject Bit14mm;
    public GameObject Bit16mm;
    public GameObject Bit18mm;
    public GameObject Bit20mm;
    public GameObject BitExtender;

    [Header("Parameters")]
    public GameObject CorrectBit;

    private GameObject BitInSocket;
    private bool IsThereABitInSocket;
    public bool TheBitIsCorrect = false;

    // Update is called once per frame
    void Update()
    {
        CheckForBit();

        if (BitSocket.GetComponent<XRSocketInteractor>().selectTarget.gameObject == null)
        {
            IsThereABitInSocket = false;
        }
        else
        {
            IsThereABitInSocket = true;
        }

        if (IsThereABitInSocket == true)
        {
            if (BitInSocket == CorrectBit && TheBitIsCorrect == false)
            {
                Debug.Log("<color=yellow>[WrenchManager.cs] </color> Correct bit is in socket.");
                //Great success. Unscrew the thing or send another bool to allow it to happen with collision.
                TheBitIsCorrect = true;
                CorrectBit.GetComponent<XRSocketInteractor>().enabled = true;
            }
            else
            {
                TheBitIsCorrect = false;
            }
        }
    }

    public void CheckForBit()
    {
        if (BitSocket.GetComponent<XRSocketInteractor>().selectTarget.gameObject != null)
        {
            BitInSocket = BitSocket.GetComponent<XRSocketInteractor>().selectTarget.gameObject;
        }
        else
        {
            //BitInSocket = null;
        }
    }
}
