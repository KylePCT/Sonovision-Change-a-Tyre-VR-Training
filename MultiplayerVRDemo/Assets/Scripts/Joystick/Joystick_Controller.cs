using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick_Controller : MonoBehaviour
{
    private Joystick_CallMovement joystickControl;

    [Header("Collisions")]
    public GameObject movingObject;

    public Collider forwardCol = null;
    public Collider backwardCol = null;
    public Collider leftCol = null;
    public Collider rightCol = null;
    public Collider upCol = null;
    public Collider downCol = null;

    [Header("Movement Limits (Will change in Play Mode)")]
    public float startXLimit = -5f; //Backwards limit.
    public float endXLimit = 5f; //Forwards limit.
    public float startYLimit = -1f; //Downwards limit.
    public float endYLimit = 1f; //Upwards limit.
    public float startZLimit = -5f; //Left limit.
    public float endZLimit = 5f; //Right limit.

    private void OnEnable()
    {
        //Set the script reference.
        joystickControl = movingObject.GetComponent<Joystick_CallMovement>();

        //Make the limits local rather than world.
        startXLimit = startXLimit + movingObject.transform.position.x;
        endXLimit = endXLimit + movingObject.transform.position.x;
        startYLimit = startYLimit + movingObject.transform.position.y;
        endYLimit = endYLimit + movingObject.transform.position.y;
        startZLimit = startZLimit + movingObject.transform.position.z;
        endZLimit = endZLimit + movingObject.transform.position.z;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Collider>() == forwardCol)
        {
            if (movingObject.transform.position.x <= endXLimit)
            {
                Debug.Log("Forward.");
                joystickControl.ObjectForward();
            }
        }

        if (other.GetComponent<Collider>() == backwardCol)
        {
            if (movingObject.transform.position.x >= startXLimit)
            {
                Debug.Log("Backward.");
                joystickControl.ObjectBackward();
            }
        }

        if (other.GetComponent<Collider>() == leftCol)
        {
            if (movingObject.transform.position.z <= endZLimit)
            {
                Debug.Log("Left.");
                joystickControl.ObjectLeft();
            }
        }

        if (other.GetComponent<Collider>() == rightCol)
        {
            if (movingObject.transform.position.z >= startZLimit)            
            {
                Debug.Log("Right.");
                joystickControl.ObjectRight();
            }
        }

        if (other.GetComponent<Collider>() == downCol)
        {
            if (movingObject.transform.position.y >= startYLimit)
            {
                Debug.Log("Down.");
                joystickControl.ObjectDown();
            }
        }

        if (other.GetComponent<Collider>() == upCol)
        {
            if (movingObject.transform.position.y <= endYLimit)
            {
                Debug.Log("Up.");
                joystickControl.ObjectUp();
            }
        }
    }
}
