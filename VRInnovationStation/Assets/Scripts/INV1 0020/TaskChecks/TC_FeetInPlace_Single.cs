using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TC_FeetInPlace_Single : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public bool IsFootInCollision;

    public PhotonView m_photonView;
    public GameObject CollisionBox;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Chassis_Foot"))
        {
            IsFootInCollision = true;
            m_photonView.RPC("UpdatePercentageUp", RpcTarget.AllBuffered);
            Debug.Log("<color=white>[TC_FeetInPlace_Single.cs] </color>" + gameObject.name + " is now in the correct place.");
        }
        else
        {
            IsFootInCollision = false;
            Debug.Log("<color=white>[TC_FeetInPlace_Single.cs] </color>" + gameObject.name + " has entered the collision and is not tagged 'Chassis_Foot'.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Chassis_Foot"))
        {
            IsFootInCollision = false;
            m_photonView.RPC("UpdatePercentageDown", RpcTarget.AllBuffered);
            Debug.Log("<color=white>[TC_FeetInPlace_Single.cs] </color>" + gameObject.name + " is no longer in the collision.");
        }
    }

    public void ActivateMeshRenderer()
    {
        CollisionBox.GetComponent<MeshRenderer>().enabled = true;
    }

    public void DeactivateMeshRenderer()
    {
        CollisionBox.GetComponent<MeshRenderer>().enabled = false;
    }

    [PunRPC]
    void UpdatePercentageUp()
    {
        FindObjectOfType<ProgressChecker>().IncreasePercentageBy(5);
    }

    [PunRPC]
    void UpdatePercentageDown()
    {
        FindObjectOfType<ProgressChecker>().DecreasePercentageBy(5);
    }
}
