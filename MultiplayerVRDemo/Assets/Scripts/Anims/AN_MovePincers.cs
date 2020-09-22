using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AN_MovePincers : MonoBehaviour
{
    public Animator L_Pincer;
    public Animator R_Pincer;
    private bool pincersMoved = false;

    private void Start()
    {
        pincersMoved = false;
    }

    public void MovePincers()
    {
        if (pincersMoved == false)
        {
            L_Pincer.Play("AN_PincerForward");
            L_Pincer.SetBool("MovePincer", true);
            R_Pincer.Play("AN_PincerForward");
            R_Pincer.SetBool("MovePincer", true);
            pincersMoved = true;
        }
    }
}
