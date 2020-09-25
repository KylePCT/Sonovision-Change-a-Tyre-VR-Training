using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictRotation : MonoBehaviour
{
    private float rotationZ;
    public GameObject RestrictedObject;
    public float zAngleMin;
    public float zAngleMax;

    private void Start()
    {
        rotationZ = RestrictedObject.transform.eulerAngles.z;
        rotationZ = (rotationZ > 180) ? rotationZ - 360 : rotationZ;
        RestrictedObject.transform.eulerAngles = new Vector3(-90f, -90f, rotationZ);
    }
    void Update()
    {
        rotationZ = RestrictedObject.transform.eulerAngles.z;
        rotationZ = (rotationZ > 180) ? rotationZ - 360 : rotationZ;

        if (rotationZ > zAngleMax)
        {
            Debug.Log("Angle reset to max. " + rotationZ + " is now " + zAngleMax);
            RestrictedObject.transform.eulerAngles = new Vector3(-90f, -90f, zAngleMax);
        }

        else if (rotationZ < zAngleMin)
        {
            Debug.Log("Angle reset to min. " + rotationZ + " is now " + zAngleMin);
            RestrictedObject.transform.eulerAngles = new Vector3(-90f, -90f, zAngleMin);
        }
    }
}
