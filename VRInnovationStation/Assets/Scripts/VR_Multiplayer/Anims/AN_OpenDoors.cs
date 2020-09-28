using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AN_OpenDoors : MonoBehaviour
{
    public Animator doorLeft;
    public Animator doorRight;
    private bool areDoorsOpen = false;

    private void Start()
    {
        areDoorsOpen = false;
    }

    public void OpenDoors()
    {
        if (areDoorsOpen == false)
        {
            doorLeft.Play("AN_LeftOpen");
            doorRight.Play("AN_RightOpen");
            doorLeft.SetBool("AN_LeftOpen", true);
            doorRight.SetBool("AN_RightOpen", true);
            areDoorsOpen = true;
        }
    }

}
