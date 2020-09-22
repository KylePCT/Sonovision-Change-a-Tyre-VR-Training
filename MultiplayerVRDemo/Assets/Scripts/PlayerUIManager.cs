using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public GameObject VRMenu_Gameobject;
    public GameObject GoHomeButton;

    //Start is called before the first frame update
    void Start()
    {
        VRMenu_Gameobject.SetActive(false);
        GoHomeButton.GetComponent<Button>().onClick.AddListener(VirtualWorldManager.Instance.LeaveRoomAndLoadHomeScene);
    }

}
