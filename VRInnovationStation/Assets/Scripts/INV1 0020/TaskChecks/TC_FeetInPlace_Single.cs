using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TC_FeetInPlace_Single : MonoBehaviour
{
    [HideInInspector]
    public bool IsFootInCollision;

    public GameObject CollisionBox;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Chassis_Foot"))
        {
            IsFootInCollision = true;
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
}
