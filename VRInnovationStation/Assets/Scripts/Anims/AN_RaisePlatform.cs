using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AN_RaisePlatform : MonoBehaviour
{
    public Animator platform;
    private bool isPlatformRaised = false;

    private void Start()
    {
        isPlatformRaised = false;
    }

    public void RaisePlatform()
    {
        if (isPlatformRaised == false)
        {
            platform.Play("AN_RaisePlatform");
            platform.SetBool("AN_RaisePlatform", true);
            isPlatformRaised = true;
        }
    }

}
