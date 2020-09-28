using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject UI_VRMenuGameobject;
    public GameObject UI_OpenWorldsGameobject;

    // Start is called before the first frame update
    void Start()
    {
        //Initially the menu should be deactivated.
        UI_VRMenuGameobject.SetActive(false);
        //UI_OpenWorldsGameobject.SetActive(false);
    }

    public void OnWorldsButtonClicked()
    {
        Debug.Log("Worlds button clicked.");

        if (UI_OpenWorldsGameobject != null)
        {
            UI_OpenWorldsGameobject.SetActive(true);
        }
    }

    public void OnHomeButtonClicked()
    {
        Debug.Log("Home button clicked.");
    }

    public void OnChangeAvatarButtonClicked()
    {
        Debug.Log("Change Avatar button clicked.");
        AvatarSelectionManager.Instance.ActivateAvatarSelectionPlatform();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
