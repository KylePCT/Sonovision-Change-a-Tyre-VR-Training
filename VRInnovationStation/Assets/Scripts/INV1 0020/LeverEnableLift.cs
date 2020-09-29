using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverEnableLift : MonoBehaviour
{
    [HideInInspector]
    public bool LiftCanMove;

    // Start is called before the first frame update
    void Start()
    {
        LiftCanMove = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HiddenCollision")
        {
            LiftCanMove = true;
        }
        else
        {
            LiftCanMove = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        LiftCanMove = false;
    }
}
