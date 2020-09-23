using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick_Movement : MonoBehaviour
{
    //This script should be added to the object which will be moved.

    //objectToFollow is the one the player can control or is the main moving object.
    public Transform objectToFollow;
    public float followSharpness = 0.1f;

    Vector3 _followOffset;
    Vector3 _previousLocation;

    void Start()
    {
        //Cache the initial offset at time of load/spawn:
        _followOffset = transform.position - objectToFollow.position;
    }

    void LateUpdate()
    {
        //Apply that offset to get a target position.
        Vector3 targetPosition = objectToFollow.position + _followOffset;

        //Keep our y position unchanged.
        targetPosition.y = transform.position.y;

        //Smooth follow.    
        transform.position += (targetPosition - transform.position) * followSharpness;
    }
}
