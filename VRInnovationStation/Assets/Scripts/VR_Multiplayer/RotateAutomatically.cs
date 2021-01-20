using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAutomatically : MonoBehaviour
{
    //Dirty way of rotating an object.
    public float spinAmount;

    public bool XAxis;
    public bool YAxis;
    public bool ZAxis;

    //Update is called once per frame
    void Update()
    {
        if (XAxis == true)
        {
            transform.Rotate(spinAmount * Time.deltaTime, 0, 0); //rotates spinAmount degrees per second around x axis
        }

        else if (YAxis == true)
        {
            transform.Rotate(0, spinAmount * Time.deltaTime, 0); //rotates spinAmount degrees per second around y axis
        }

        else if (ZAxis == true)
        {
            transform.Rotate(0, 0, spinAmount * Time.deltaTime); //rotates spinAmount degrees per second around z axis
        }

        else
        {
            transform.Rotate(0, spinAmount * Time.deltaTime, 0); //rotates spinAmount degrees per second around y axis
        }
    }
}