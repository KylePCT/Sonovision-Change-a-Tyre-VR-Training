using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Photon.Realtime;

public class TC_GotPPE : MonoBehaviourPunCallbacks
{
    public GameObject Player;
    public GameObject PPE_Helmet;
    public GameObject PPE_HighVis;
    public PhotonView m_photonView;

    [Space(10)]

    public UI_Instruction Instruction;

    public GameObject InstructionCanvas;

    private static bool GotHelmet = false;
    private static bool GotHighVis = false;

    [Space(10)]

    public ParticleSystem RemovedParticles;

    [Space(10)]

    public GameObject[] ObjectsToShowWhenPPEIsOn;

    private AudioManager AudioManager;
    private ProgressChecker ProgressChecker;

    [HideInInspector]
    public bool IsSafeToWork = false;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager = FindObjectOfType<AudioManager>();
        ProgressChecker = FindObjectOfType<ProgressChecker>();

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
        //If the player grabs it...
        if (collision.gameObject.tag == "Player")
        {
            AudioManager.PlaySound("UI_Complete");

            if (gameObject.name == "PPE_Goggles")
            {
                GotHelmet = true;
                Debug.Log("<color=orange>[TC_GotPPE.cs]</color> Player has picked up goggles; GotHelmet is now: " + GotHelmet + ".");

                //Instantiate particles and set to unactive.
                CheckPPE();
                Instantiate(RemovedParticles, PPE_Helmet.transform.position, PPE_Helmet.transform.rotation);
                m_photonView.RPC("UpdatePercentageUp", RpcTarget.AllBufferedViaServer);
                PPE_Helmet.SetActive(false);
            }

            if (gameObject.name == "PPE_HighVis")
            {
                GotHighVis = true;
                Debug.Log("<color=orange>[TC_GotPPE.cs]</color> Player has picked up high visibility jacket; GotHighVis is now: " + GotHighVis + ".");

                CheckPPE();
                Instantiate(RemovedParticles, PPE_HighVis.transform.position, PPE_HighVis.transform.rotation);
                m_photonView.RPC("UpdatePercentageUp", RpcTarget.AllBufferedViaServer);
                PPE_HighVis.SetActive(false);
            }
        }
    }

    void CheckPPE()
    {
        //When both PPE objects are attained, you can do VR things.
        if (GotHelmet || GotHighVis)
        {
            //Debug check.
            Debug.Log("<color=orange>[TC_GotPPE.cs]</color> GotHelmet is " + GotHelmet + ". GotHighVis is " + GotHighVis + ".");

            //If you have both PPE, allow interactions.
            if (GotHelmet && GotHighVis)
            {
                Debug.Log("<color=orange>[TC_GotPPE.cs]</color> All PPE has been picked up.");
                IsSafeToWork = true;
                Instruction.IsTaskComplete = true;

                InstructionCanvas.GetComponent<AssignCanvasEventCamera>().AttachCamera();

                Player.GetComponentInChildren<XRDirectInteractor>().enableInteractions = true;
                Player.GetComponentInChildren<XRRayInteractor>().enableInteractions = true;
                FindObjectOfType<ProgressChecker>().ChangePercentageTo(5);

                ShowExtrasWithPPEOn();
                Debug.Log("<color=orange>[TC_GotPPE.cs]</color> Interactions activated.");
            }
        }

        else
        {
            Debug.Log("<color=orange>[TC_GotPPE.cs]</color> Player does not have any PPE yet.");
        }
    }

    //For any extra objects which need to be turned on.
    void ShowExtrasWithPPEOn()
    {
        foreach (GameObject i in ObjectsToShowWhenPPEIsOn)
        {
            i.SetActive(true);
        }
    }

    //Photon progress RPCCall updates.
    [PunRPC]
    void UpdatePercentageUp()
    {
        ProgressChecker.IncreasePercentageBy(1);
    }
}
