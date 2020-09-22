using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AN_RocketMachineAnims : MonoBehaviour
{
    public Animator doorLeft;
    public Animator doorRight;
    private bool areDoorsOpen;

    public Animator platform;
    private bool isPlatformRaised;

    public Animator L_Pincer;
    public Animator R_Pincer;
    private bool pincersMoved;

    public Animator U_Pincer;
    private bool tipLowered;

    public GameObject rocketBase;
    public GameObject rocketWing_L;
    public GameObject rocketWing_R;
    public GameObject rocketTip;
    public GameObject rocketGroupForParent;

    private void Start()
    {
        areDoorsOpen = false;
        isPlatformRaised = false;
        pincersMoved = false;
        tipLowered = false;
    }

    public void OpenDoors()
    {
        if (areDoorsOpen == false)
        {
            areDoorsOpen = true;
            doorLeft.Play("AN_LeftOpen");
            doorRight.Play("AN_RightOpen");
            doorLeft.SetBool("AN_LeftOpen", true);
            doorRight.SetBool("AN_RightOpen", true);

            Debug.Log("[AN_RocketMachineAnims.cs] Doors opened.");
        }
    }

    public void RaisePlatform()
    {
        if (isPlatformRaised == false)
        {
            isPlatformRaised = true;
            platform.Play("AN_RaisePlatform");
            platform.SetBool("AN_RaisePlatform", true);

            Debug.Log("[AN_RocketMachineAnims.cs] Platform raised.");
        }
    }

    public void MovePincers()
    {
        if (pincersMoved == false)
        {
            pincersMoved = true;
            L_Pincer.Play("AN_PincerForward");
            L_Pincer.SetBool("MovePincer", true);
            R_Pincer.Play("AN_PincerForward");
            R_Pincer.SetBool("MovePincer", true);

            Debug.Log("[AN_RocketMachineAnims.cs] Pincers moved side elements.");
        }
    }

    public void LowerTip()
    {
        if (tipLowered == false)
        {
            tipLowered = true;
            U_Pincer.Play("AN_LowerTip");
            U_Pincer.SetBool("LowerTip", true);

            Debug.Log("[AN_RocketMachineAnims.cs] Tip lowered.");
        }
    }

    public void MakeRocketGrabbable()
    {
        if (tipLowered == true)
        {
            ForceRocketGrabbable();
        }
    }

    //Basically if it's being difficult.
    public void ForceRocketGrabbable()
    {
        rocketGroupForParent.transform.parent = null;
        rocketBase.transform.parent = null;
        rocketTip.transform.parent = null;
        rocketWing_L.transform.parent = null;
        rocketWing_R.transform.parent = null;

        rocketBase.transform.SetParent(rocketGroupForParent.transform);
        rocketTip.transform.SetParent(rocketGroupForParent.transform);
        rocketWing_L.transform.SetParent(rocketGroupForParent.transform);
        rocketWing_R.transform.SetParent(rocketGroupForParent.transform);

        rocketGroupForParent.GetComponent<SphereCollider>().enabled = true;
        Debug.Log("[AN_RocketMachineAnims.cs] Group collider enabled.");

        //Add collision.
        rocketGroupForParent.AddComponent<BoxCollider>();
        rocketGroupForParent.GetComponent<BoxCollider>().size = new Vector3(0.47f, 0.23f, 0.83f); //Good size for the rocket.

        rocketGroupForParent.GetComponent<Rigidbody>().useGravity = true;
        Debug.Log("[AN_RocketMachineAnims.cs] Rocket is now grabbable.");

        //Reverse the animations for when they return.
        L_Pincer.SetBool("MovePincer", false);
        L_Pincer.SetBool("ReturnPincer", true);

        R_Pincer.SetBool("MovePincer", false);
        R_Pincer.SetBool("ReturnPincer", true);

        U_Pincer.SetBool("LowerTip", false);
        U_Pincer.SetBool("ReturnTip", true);

        L_Pincer.Play("AN_PincerForward");
        R_Pincer.Play("AN_PincerForward");
        U_Pincer.Play("AN_LowerTip");
    }
}
