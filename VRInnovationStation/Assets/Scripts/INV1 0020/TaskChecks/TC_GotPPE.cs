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

    [SerializeField]
    private GameObject InstructionCanvas;

    private bool GotHelmet;
    private bool GotHighVis;

    [HideInInspector]
    public bool IsSafeToWork = false;

    // Start is called before the first frame update
    void Start()
    {
        //If it isn't safe to work (no PPE) then you can't do any VR interactions.
        if (!IsSafeToWork)
        {
            //Player.GetComponentInChildren<XRDirectInteractor>().enableInteractions = false;
            //Player.GetComponentInChildren<ControllerManager>().enabled = false;
        }
    }

    private void LateUpdate()
    {
        InstructionCanvas = GameObject.Find("[INSTRUCTION UI]/[CANVASES]/UI_Tablet_v1/GeneratedCanvases/UI_Canvas_2a_PPE");
        Instruction.IsTaskComplete = false;

        InstructionCanvas.gameObject.transform.Find("InstructionPanel/Forward").GetComponent<Button>().interactable = false;
    }

    //Grab PPE when colliding.
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && gameObject.name == "PPE_Goggles")
        {
            Debug.Log("[Task Check] Player has picked up goggles.");
            PPE_Helmet.gameObject.SetActive(false);
            Player.gameObject.transform.Find("Generic VR Player/Avatar_001/Body1/Avatar_HighVis_v_Sono").GetComponent<GameObject>().layer = 31;
            GotHelmet = true;

        }

        if (collision.gameObject.tag == "Player" && gameObject.name == "PPE_HighVis")
        { 
            Debug.Log("[Task Check] Player has picked up high visibility jacket.");
            PPE_HighVis.gameObject.SetActive(false);
            GotHighVis = true;

            CheckPPE();
        }
    }

    void CheckPPE()
    {
        //When both PPE objects are attained, you can do VR things.
        if (GotHelmet == true && GotHighVis == true)
        {
            IsSafeToWork = true;
            Instruction.IsTaskComplete = true;
            InstructionCanvas.gameObject.transform.Find("InstructionPanel/Forward").GetComponent<Button>().interactable = true;

            //Player.GetComponentInChildren<XRDirectInteractor>().enableInteractions = true;
        }

        else
        {
            IsSafeToWork = false;
        }
    }
}
