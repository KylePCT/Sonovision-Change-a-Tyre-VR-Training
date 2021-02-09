using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

//Checks each individual foot.
public class TC_FeetInPlace_Single : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public bool IsFootInCollision;

    public PhotonView m_photonView;
    public GameObject CollisionBox;

    //If a foot enters the collision...
    private void OnTriggerEnter(Collider other)
    {
        //Check tag.
        if (other.gameObject.CompareTag("Chassis_Foot"))
        {
            //Set the collision to be true and update the progress UI.
            IsFootInCollision = true;
            m_photonView.RPC("UpdatePercentageUp", RpcTarget.AllBuffered);
            Debug.Log("<color=white>[TC_FeetInPlace_Single.cs] </color>" + gameObject.name + " is now in the correct place.");
        }
        else
        {
            //If a random thing enters the collision, don't set variables.
            IsFootInCollision = false;
            Debug.Log("<color=white>[TC_FeetInPlace_Single.cs] </color>" + gameObject.name + " has entered the collision and is not tagged 'Chassis_Foot'.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Otherwise just remove the percentage stated prior.
        if (other.gameObject.CompareTag("Chassis_Foot"))
        {
            IsFootInCollision = false;
            m_photonView.RPC("UpdatePercentageDown", RpcTarget.AllBuffered);
            Debug.Log("<color=white>[TC_FeetInPlace_Single.cs] </color>" + gameObject.name + " is no longer in the collision.");
        }
    }

    //Turn meshes on and off appropriately.
    public void ActivateMeshRenderer()
    {
        CollisionBox.GetComponent<MeshRenderer>().enabled = true;
    }

    public void DeactivateMeshRenderer()
    {
        CollisionBox.GetComponent<MeshRenderer>().enabled = false;
    }

    //Progress check RPCCalls for Photon Multiplayer.
    [PunRPC]
    void UpdatePercentageUp()
    {
        FindObjectOfType<ProgressChecker>().IncreasePercentageBy(2);
    }

    [PunRPC]
    void UpdatePercentageDown()
    {
        FindObjectOfType<ProgressChecker>().DecreasePercentageBy(2);
    }
}
