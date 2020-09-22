using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AN_LowerTip : MonoBehaviour
{
    public Animator U_Pincer;
    private bool tipLowered = false;

    private void Start()
    {
        tipLowered = false;
    }

    public void LowerTip()
    {
        if (tipLowered == false)
        {
            U_Pincer.Play("AN_LowerTip");
            U_Pincer.SetBool("LowerTip", true);
            tipLowered = true;
        }
    }
}
