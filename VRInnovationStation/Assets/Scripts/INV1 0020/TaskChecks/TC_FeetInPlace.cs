using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TC_FeetInPlace : MonoBehaviourPunCallbacks
{
    public PhotonView m_photonView;

    public GameObject Col_LeftFrontArmPlace;
    public GameObject Col_LeftBackArmPlace;
    public GameObject Col_RightFrontArmPlace;
    public GameObject Col_RightBackArmPlace;

    public GameObject UI_ProgressTask;

    private bool UI_ProgressTaskComplete;

    [HideInInspector]
    public bool AreAllFeetInPlace = false;

    private void Start()
    {
        DeactivateAllMeshRenderers();
        UI_ProgressTaskComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Col_LeftFrontArmPlace.GetComponent<TC_FeetInPlace_Single>().IsFootInCollision == true && 
            Col_LeftBackArmPlace.GetComponent<TC_FeetInPlace_Single>().IsFootInCollision == true &&
            Col_RightFrontArmPlace.GetComponent<TC_FeetInPlace_Single>().IsFootInCollision == true &&
            Col_RightBackArmPlace.GetComponent<TC_FeetInPlace_Single>().IsFootInCollision == true)
        {
            AreAllFeetInPlace = true;
            m_photonView.RPC("SetActiveUIElements", RpcTarget.AllBuffered);
            DeactivateAllMeshRenderers();
            Debug.Log("<color=white>[TC_FeetInPlace.cs] Lift can now be raised. All four feet are in place.</color>");
        }
        else
        {
            AreAllFeetInPlace = false;
        }
    }

    [PunRPC]
    void SetActiveUIElements()
    {
        if (UI_ProgressTaskComplete == false)
        {
            UI_ProgressTask.transform.gameObject.SetActive(true);
            FindObjectOfType<AudioManager>().PlaySound("UI_Complete");
            FindObjectOfType<ProgressChecker>().ChangePercentageTo(25);
            UI_ProgressTaskComplete = true;
        }
    }

    public void DeactivateAllMeshRenderers()
    {
        Col_LeftFrontArmPlace.GetComponent<MeshRenderer>().enabled = false;
        Col_LeftBackArmPlace.GetComponent<MeshRenderer>().enabled = false;
        Col_RightFrontArmPlace.GetComponent<MeshRenderer>().enabled = false;
        Col_RightBackArmPlace.GetComponent<MeshRenderer>().enabled = false;
    }
}
