using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick_CallMovement : MonoBehaviour
{
    public float speed = 2f;

    //Debug.
    //void Update()
    //{
    //    if (Input.GetKey(KeyCode.DownArrow))
    //    {
    //        ObjectBackward();
    //    }
    //    if (Input.GetKey(KeyCode.UpArrow))
    //    {
    //        ObjectForward();
    //    }
    //    if (Input.GetKey(KeyCode.LeftArrow))
    //    {
    //        ObjectLeft();
    //    }
    //    if (Input.GetKey(KeyCode.RightArrow))
    //    {
    //        ObjectRight();
    //    }
    //}

    public void ObjectForward()
    {
        Vector3 pos = transform.position;
        pos.x = pos.x + (speed / 100);
        transform.position = pos;
    }

    public void ObjectBackward()
    {
        Vector3 pos = transform.position;
        pos.x = pos.x - (speed / 100);
        transform.position = pos;
    }

    public void ObjectLeft()
    {
        Vector3 pos = transform.position;
        pos.z = pos.z + (speed / 100);
        transform.position = pos;
    }

    public void ObjectRight()
    {
        Vector3 pos = transform.position;
        pos.z = pos.z - (speed / 100);
        transform.position = pos;
    }

    public void ObjectUp()
    {
        Vector3 pos = transform.position;
        pos.y = pos.y + (speed / 100);
        transform.position = pos;
    }

    public void ObjectDown()
    {
        Vector3 pos = transform.position;
        pos.y = pos.y - (speed / 100);
        transform.position = pos;
    }
}
