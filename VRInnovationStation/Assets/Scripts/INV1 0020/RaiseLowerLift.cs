using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class RaiseLowerLift : MonoBehaviour
{
    //Public bools to tell the objects if it's being raised or lowered.
    public bool MakeLiftRaise;
    public bool MakeLiftLower;

    private bool IsRaising;
    private bool IsLowering;

    [Header("GameObject References")]
    public GameObject Lift;
    public GameObject ButtonConfirmationIndication;
    public GameObject ButtonRaise;
    public GameObject ButtonLower;
    [Space(10)]
    public GameObject MainLever;
    public GameObject MainLeverCollision;
    [Space(10)]
    public TextMeshProUGUI TextDistanceFromFloor;

    [Header("Movement Variables (z Axis)")]
    public float LowestPositionLimit;
    public float HighestPositionLimit;

    [Header("Measurements in mm (Unity Units are in Metres)")]
    public float TargetHeightToRemoveWheel;
    public float TargetHeightLeeway = 5f;

    [Space(10)]
    public float LiftSpeed;

    public LeverEnableLift EnableLift;
    private bool CanMoveLift = false;
    private bool CanRemoveWheel = false;

    [Header("Visual Materials")]
    public Material DefaultMat;
    [Space(10)]
    public Material WheelCantBeMoved;
    public Material WheelCanBeMoved;

    // Start is called before the first frame update
    void Start()
    {
        IsRaising = false;
        IsLowering = false;

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
            if (EnableLift.LiftCanMove == true)
            {
                IsLowering = true;
                IsRaising = false;
                Debug.Log("Lowering Lift.");
            }
            else
            {
                //No move. Maybe a sad noise.
                IsRaising = false;
                IsLowering = false;
                Debug.Log("Stopped moving, lever is not in place.");
            }
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
        TextDistanceFromFloor.SetText((Lift.transform.position.y * 100).ToString("F2") + "mm");

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
            ButtonConfirmationIndication.GetComponent<MeshRenderer>().material = WheelCanBeMoved;
        }
        else
        {
            CanRemoveWheel = false;
            ButtonConfirmationIndication.GetComponent<MeshRenderer>().material = WheelCantBeMoved;
        }
    }

    public void RaiseLift()
    {
        if (Lift.transform.position.y < HighestPositionLimit)
        {
            ButtonRaise.GetComponent<MeshRenderer>().material = WheelCanBeMoved;
            ButtonLower.GetComponent<MeshRenderer>().material = DefaultMat;
            Lift.transform.position += (Vector3.up * LiftSpeed * Time.deltaTime);
        }
    }

    public void LowerLift()
    {
        if (Lift.transform.position.y > LowestPositionLimit)
        {
            ButtonLower.GetComponent<MeshRenderer>().material = WheelCanBeMoved;
            ButtonRaise.GetComponent<MeshRenderer>().material = DefaultMat;

            if (EnableLift.LiftCanMove == true)
            {
                Lift.transform.position += (Vector3.down * LiftSpeed * Time.deltaTime);
            }
        }
    }

    //This void allows the object to be synced using Photon View.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //The recieve order MUST be the send as the same order.
        if (stream.IsWriting) //The Local Client uses this.
        {
            //stream.SendNext(VARIABLE TO SYNC);
            stream.SendNext(MakeLiftRaise);
            stream.SendNext(MakeLiftLower);
            stream.SendNext(IsRaising);
            stream.SendNext(IsLowering);
        }
        else //The remote client uses this.
        {
            //This should get the data from the network.
            //this.VARIABLE = (VARIABLE TYPE)stream.RecieveNext();
            this.MakeLiftRaise = (bool)stream.ReceiveNext();
            this.MakeLiftLower = (bool)stream.ReceiveNext();
            this.IsRaising = (bool)stream.ReceiveNext();
            this.IsLowering = (bool)stream.ReceiveNext();
        }
    }

    //Public voids to call from buttons.
    #region Public Voids
    public void MakeRaise()
    {
        IsLowering = false;
        IsRaising = true;
    }

    public void MakeLower()
    {
        IsRaising = false;
        IsLowering = true;
    }

    public void StopMoving()
    {
        IsRaising = false;
        IsLowering = false;
    }
    #endregion
}
