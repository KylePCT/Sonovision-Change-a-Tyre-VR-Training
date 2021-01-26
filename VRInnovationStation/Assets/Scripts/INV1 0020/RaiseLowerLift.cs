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

    public TC_FeetInPlace TaskCheck;
    public LeverEnableLift EnableLift;
    private bool CanMoveLift = false;
    private bool CanRemoveWheel = false;

    [Header("Visual Materials")]
    public Material DefaultMat;
    [Space(10)]
    public Material WheelCantBeMoved;
    public Material WheelCanBeMoved;

    [Space(10)]
    public GameObject UI_LiftIsRaisedTaskButton;
    public PhotonView m_photonView;

    private bool UI_ProgressTaskComplete;

    // Start is called before the first frame update
    void Start()
    {
        UI_ProgressTaskComplete = false;

        IsRaising = false;
        IsLowering = false;

        //Auto-generate limits based on current position.
        LowestPositionLimit = LowestPositionLimit + Lift.transform.position.y;
        HighestPositionLimit = HighestPositionLimit + Lift.transform.position.y;
        TargetHeightToRemoveWheel = TargetHeightToRemoveWheel + Lift.transform.position.y;

        Vector3 tempLow = new Vector3(Lift.transform.position.x, LowestPositionLimit, Lift.transform.position.z);
        Lift.transform.position = tempLow;
    }

    //When the trigger is entered, check what values are correct and run the appropriate code.
    void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject.tag);
        //If 'raise' is selected.
        if (collision.gameObject.tag == "Player_FT" && MakeLiftRaise == true)
        {
            IsRaising = true;
            IsLowering = false;
            FindObjectOfType<AudioManager>().PlaySound("CarLiftMoving");

            Debug.Log("Raising Lift.");
        }

        //If 'lower' is selected.
        else if (collision.gameObject.tag == "Player_FT" && MakeLiftLower == true)
        {
            if (EnableLift.LiftCanMove == true)
            {
                IsLowering = true;
                IsRaising = false;
                FindObjectOfType<AudioManager>().PlaySound("CarLiftMoving");

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

    //Reset values when buttons are not pressed through the trigger.
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player_FT")
        {
            IsRaising = false;
            IsLowering = false;
            ButtonRaise.GetComponent<MeshRenderer>().material = DefaultMat;
            ButtonLower.GetComponent<MeshRenderer>().material = DefaultMat;
            FindObjectOfType<AudioManager>().PlaySound("CarLiftStop");

            Debug.Log("Stopped moving.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //-60 is just to get it nearish 0.
        TextDistanceFromFloor.SetText(((Lift.transform.position.y * 100) - 60).ToString("F2") + "cm");

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
            UI_LiftIsRaisedTaskButton.SetActive(true);
            m_photonView.RPC("SetActiveUIElements", RpcTarget.AllBuffered);
        }
        else
        {
            CanRemoveWheel = false;
            ButtonConfirmationIndication.GetComponent<MeshRenderer>().material = WheelCantBeMoved;
        }
    }

    //Raise lift position and set RPC values for multiplayer UI;
    public void RaiseLift()
    {
        if (Lift.transform.position.y < HighestPositionLimit && TaskCheck.AreAllFeetInPlace)
        {
            ButtonRaise.GetComponent<MeshRenderer>().material = WheelCanBeMoved;

            ButtonLower.GetComponent<MeshRenderer>().material = DefaultMat;
            Lift.transform.position += (Vector3.up * LiftSpeed * Time.deltaTime);

            UI_LiftIsRaisedTaskButton.SetActive(true);
        }

        else
        {
            FindObjectOfType<AudioManager>().StopPlaying("CarLiftMoving");
        }
    }

    //Lower lift position.
    public void LowerLift()
    {
        if (Lift.transform.position.y > LowestPositionLimit && TaskCheck.AreAllFeetInPlace)
        {
            ButtonLower.GetComponent<MeshRenderer>().material = WheelCanBeMoved;
            ButtonRaise.GetComponent<MeshRenderer>().material = DefaultMat;

            //If the lever is down...
            if (EnableLift.LiftCanMove == true)
            {
                Lift.transform.position += (Vector3.down * LiftSpeed * Time.deltaTime);
            }
        }

        else
        {
            FindObjectOfType<AudioManager>().StopPlaying("CarLiftMoving");
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

    //Photon multiplayer; sync UI and SFX.
    [PunRPC]
    void SetActiveUIElements()
    {
        if (UI_ProgressTaskComplete == false)
        {
            UI_LiftIsRaisedTaskButton.transform.gameObject.SetActive(true);
            FindObjectOfType<AudioManager>().PlaySound("UI_Complete");
            FindObjectOfType<ProgressChecker>().ChangePercentageTo(30);
            UI_ProgressTaskComplete = true;
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
