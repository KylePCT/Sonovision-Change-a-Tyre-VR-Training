using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class TC_GotPPE : MonoBehaviour
{
    public GameObject Player;
    public GameObject PPE_Helmet;
    public GameObject PPE_HighVis;

    public UI_Instruction Instruction;

    public GameObject InstructionCanvas;

    private static bool GotHelmet = false;
    private static bool GotHighVis = false;

    public ParticleSystem RemovedParticles;

    [HideInInspector]
    public bool IsSafeToWork = false;

    // Start is called before the first frame update
    void Start()
    {
        //If it isn't safe to work (no PPE) then you can't do any VR interactions.
        if (!IsSafeToWork)
        {
            Player.GetComponentInChildren<XRDirectInteractor>().enableInteractions = false;
            Player.GetComponentInChildren<XRRayInteractor>().enableInteractions = false;
        }
    }

    //Grab PPE when colliding.
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (gameObject.name == "PPE_Goggles")
            {
                GotHelmet = true;
                Debug.Log("[Task Check] Player has picked up goggles; GotHelmet is now: " + GotHelmet + ".");

                CheckPPE();
                Instantiate(RemovedParticles, PPE_Helmet.transform.position, PPE_Helmet.transform.rotation);
                PPE_Helmet.SetActive(false);
            }

            if (gameObject.name == "PPE_HighVis")
            {
                GotHighVis = true;
                Debug.Log("[Task Check] Player has picked up high visibility jacket; GotHighVis is now: " + GotHighVis + ".");

                CheckPPE();
                Instantiate(RemovedParticles, PPE_HighVis.transform.position, PPE_HighVis.transform.rotation);
                PPE_HighVis.SetActive(false);
            }
        }
    }

    void CheckPPE()
    {
        //When both PPE objects are attained, you can do VR things.
        if (GotHelmet || GotHighVis)
        {
            Debug.Log("[Task Check] GotHelmet is " + GotHelmet + ". GotHighVis is " + GotHighVis + ".");

            if (GotHelmet && GotHighVis)
            {
                Debug.Log("[Task Check] All PPE has been picked up.");
                IsSafeToWork = true;
                Instruction.IsTaskComplete = true;

                InstructionCanvas.GetComponent<AssignCanvasEventCamera>().AttachCamera();

                Player.GetComponentInChildren<XRDirectInteractor>().enableInteractions = true;
                Player.GetComponentInChildren<XRRayInteractor>().enableInteractions = true;
                Debug.Log("[Task Check] Interactions activated.");
            }
        }

        else
        {
            Debug.Log("[Task Check] Player does not have any PPE yet.");
        }
    }
}
