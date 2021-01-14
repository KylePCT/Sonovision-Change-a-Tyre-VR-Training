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

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HiddenCollision")
        {
            ArmsCanMove = true;
            FindObjectOfType<AudioManager>().PlaySound("MetalClang");
            MoveArms();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "HiddenCollision")
        {
            ArmsCanMove = true;
            MoveArms();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ArmsCanMove = false;
        FindObjectOfType<AudioManager>().PlaySound("MetalClang");
        ArmHandle.GetComponent<BoxCollider>().enabled = false;
    }

    public void MoveArms()
    {
        if (ArmsCanMove == true)
        {
            ArmHandle.GetComponent<BoxCollider>().enabled = true;
            Debug.Log("Arms can be moved.");
        }
    }
}
