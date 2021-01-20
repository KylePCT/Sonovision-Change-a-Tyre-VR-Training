using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableArms : MonoBehaviour
{
    public GameObject ToothLever;
    public GameObject ArmHandle;

    public GameObject TopCollider;

    private bool ArmsCanMove;

    // Start is called before the first frame update
    void Start()
    {
        ArmsCanMove = false;
        ArmHandle.GetComponent<BoxCollider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the gameObject is the hidden collision, allow the arms to move.
        if (other.gameObject.tag == "HiddenCollision")
        {
            ArmsCanMove = true;
            FindObjectOfType<AudioManager>().PlaySound("MetalClang");
            MoveArms();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //If the gameObject is the hidden collision, allow the arms to move.
        if (other.gameObject.tag == "HiddenCollision")
        {
            ArmsCanMove = true;
            MoveArms();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ArmsCanMove = false;
        ArmHandle.GetComponent<BoxCollider>().enabled = false;
    }

    //If the arms can be moved, allow their colliders to work and therefore be interacted with.
    public void MoveArms()
    {
        if (ArmsCanMove == true)
        {
            ArmHandle.GetComponent<BoxCollider>().enabled = true;
            //Debug.Log("Arms can be moved.");
        }
    }
}
