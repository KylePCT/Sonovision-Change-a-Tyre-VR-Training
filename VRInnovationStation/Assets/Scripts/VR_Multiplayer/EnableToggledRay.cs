using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EnableToggledRay : MonoBehaviour
{
    private GameObject Controller;

    public GameObject RayHand;
    public GameObject DirectHand;

    // Start is called before the first frame update
    void Start()
    {
        Controller = this.gameObject;
    }

    public void ActivateRay()
    {
        RayHand.SetActive(true);
        RayHand.GetComponent<XRRayInteractor>().enabled = true;
        RayHand.GetComponent<XRBaseInteractor>().enableInteractions = true;
    }

    public void DeactivateRay()
    {
        RayHand.GetComponent<XRRayInteractor>().enabled = false;
        RayHand.GetComponent<XRBaseInteractor>().enableInteractions = false;
        RayHand.SetActive(false);
    }
}
