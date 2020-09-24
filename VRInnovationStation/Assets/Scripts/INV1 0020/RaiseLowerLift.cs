using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RaiseLowerLift : MonoBehaviour
{
    //Public bools to tell the objects if it's being raised or lowered.
    public bool MakeLiftRaise;
    public bool MakeLiftLower;

    private bool IsRaising;
    private bool IsLowering;

    [Header("GameObject References")]
    public GameObject Lift;
    public GameObject Button_ConfirmationIndication;
    public GameObject Button_Raise;
    public GameObject Button_Lower;

    public TextMeshProUGUI Text_DistanceFromFloor;

    [Header("Movement Variables (z Axis)")]
    public float LowestPositionLimit;
    public float HighestPositionLimit;

    [Header("Measurements in mm (Unity Units are in Metres)")]
    public float TargetHeightToRemoveWheel;
    public float TargetHeightLeeway = 5f;

    [Space(10)]
    public float LiftSpeed;

    private bool CanRemoveWheel = false;

    [Header("Visual Materials")]
    public Material WheelCantBeMoved;
    public Material WheelCanBeMoved;

    // Start is called before the first frame update
    void Start()
    {
        LowestPositionLimit = LowestPositionLimit + Lift.transform.position.y;
        HighestPositionLimit = HighestPositionLimit + Lift.transform.position.y;
        TargetHeightToRemoveWheel = TargetHeightToRemoveWheel + Lift.transform.position.y;

        Vector3 tempLow = new Vector3(Lift.transform.position.x, LowestPositionLimit, Lift.transform.position.z);
        Lift.transform.position = tempLow;
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Player" && MakeLiftRaise == true)
        {
            IsRaising = true;
            IsLowering = false;
            Debug.Log("Raising Lift.");
        }

        else if (collision.gameObject.tag == "Player" && MakeLiftLower == true)
        {
            IsLowering = true;
            IsRaising = false;
            Debug.Log("Lowering Lift.");
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            IsRaising = false;
            IsLowering = false;
            Debug.Log("Stopped moving.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Text_DistanceFromFloor.SetText((Lift.transform.position.y * 100).ToString("F2") + "mm");

        if (IsRaising)
        {
            RaiseLift();
        }

        if (IsLowering)
        {
            LowerLift();
        }

        //Movement.
        if (Lift.transform.position.y <= (TargetHeightToRemoveWheel + TargetHeightLeeway) && Lift.transform.position.y >= (TargetHeightToRemoveWheel - TargetHeightLeeway))
        {
            CanRemoveWheel = true;
            Button_ConfirmationIndication.GetComponent<MeshRenderer>().material = WheelCanBeMoved;
        }
        else
        {
            CanRemoveWheel = false;
            Button_ConfirmationIndication.GetComponent<MeshRenderer>().material = WheelCantBeMoved;
        }
    }

    public void RaiseLift()
    {
        if (Lift.transform.position.y < HighestPositionLimit)
        {
            Lift.transform.position += (Vector3.up * LiftSpeed * Time.deltaTime);
            Button_Raise.GetComponent<MeshRenderer>().material = WheelCanBeMoved;
            Button_Lower.GetComponent<MeshRenderer>().material = WheelCantBeMoved;
        }
    }

    public void LowerLift()
    {
        if (Lift.transform.position.y > LowestPositionLimit)
        {
            Lift.transform.position += (Vector3.down * LiftSpeed * Time.deltaTime);
            Button_Raise.GetComponent<MeshRenderer>().material = WheelCantBeMoved;
            Button_Lower.GetComponent<MeshRenderer>().material = WheelCanBeMoved;
        }
    }

    //Public voids to call from buttons.
    #region Public Voids
    public void MakeRaise()
    {
        IsRaising = true;
    }

    public void MakeLower()
    {
        IsLowering = true;
    }

    public void StopMoving()
    {
        IsRaising = false;
        IsLowering = false;
    }
    #endregion
}
