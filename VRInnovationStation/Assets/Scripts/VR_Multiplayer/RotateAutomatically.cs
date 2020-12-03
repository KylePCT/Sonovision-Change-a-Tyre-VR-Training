using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAutomatically : MonoBehaviour
{
    public float spinAmount;

    public bool XAxis;
    public bool YAxis;
    public bool ZAxis;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (XAxis == true)
        {
            transform.Rotate(spinAmount * Time.deltaTime, 0, 0); //rotates 50 degrees per second around x axis
        }

        else if (YAxis == true)
        {
            transform.Rotate(0, spinAmount * Time.deltaTime, 0); //rotates 50 degrees per second around y axis
        }

        else if (ZAxis == true)
        {
            transform.Rotate(0, 0, spinAmount * Time.deltaTime); //rotates 50 degrees per second around z axis
        }

        else
        {
            transform.Rotate(0, spinAmount * Time.deltaTime, 0); //rotates 50 degrees per second around y axis
        }
    }
}