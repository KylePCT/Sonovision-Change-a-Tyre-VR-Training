using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TC_FeetInPlace : MonoBehaviour
{
    public GameObject Col_LeftFrontArmPlace;
    public GameObject Col_LeftBackArmPlace;
    public GameObject Col_RightFrontArmPlace;
    public GameObject Col_RightBackArmPlace;

    [HideInInspector]
    public bool AreAllFeetInPlace = false;

    private void Start()
    {
        DeactivateAllMeshRenderers();
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
            DeactivateAllMeshRenderers();
            Debug.Log("<color=white><b>[TC_FeetInPlace.cs] LIFT CAN NOW BE RAISED.</b> All four feet are in place.</color>");
        }
        else
        {
            AreAllFeetInPlace = false;
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
