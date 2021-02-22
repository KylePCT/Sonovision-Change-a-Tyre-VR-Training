using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LeverEnableLift : MonoBehaviour, Photon.Pun.IPunObservable
{
    [HideInInspector]
    public bool LiftCanMove;

    public GameObject UI_ButtonForward;

    // Start is called before the first frame update
    void Start()
    {
        LiftCanMove = false;
        UI_ButtonForward.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HiddenCollision")
        {
            LiftCanMove = true;
            FindObjectOfType<AudioManager>().PlaySound("UI_PercentUp");
            UI_ButtonForward.SetActive(true);

        }
    }

    private void OnTriggerStay(Collider other)
    {
        LiftCanMove = true;
        UI_ButtonForward.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        LiftCanMove = false;
        UI_ButtonForward.SetActive(false);
        FindObjectOfType<AudioManager>().PlaySound("UI_PercentDown");
    }

    //This void allows the object to be synced using Photon View.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //The recieve order MUST be the send as the same order.
        if (stream.IsWriting) //The Local Client uses this.
        {
            //stream.SendNext(VARIABLE TO SYNC);
            stream.SendNext(LiftCanMove);
        }
        else //The remote client uses this.
        {
            //This should get the data from the network.
            //this.VARIABLE = (VARIABLE TYPE)stream.RecieveNext();
            this.LiftCanMove = (bool)stream.ReceiveNext();
        }
    }
}
