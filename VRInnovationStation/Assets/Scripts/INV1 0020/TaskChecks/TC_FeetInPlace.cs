using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TC_FeetInPlace : MonoBehaviour
{
    public GameObject Col_LeftFrontArmPlace;
    public GameObject Col_LeftBackArmPlace;
    public GameObject Col_RightFrontArmPlace;
    public GameObject Col_RightBackArmPlace;

    private BoxCollider LeftFrontCol;
    private BoxCollider LeftBackCol;
    private BoxCollider RightFrontCol;
    private BoxCollider RightBackCol;

    private bool IsLeftFrontInPlace = false;
    private bool IsLeftBackInPlace = false;
    private bool IsRightFrontInPlace = false;
    private bool IsRightBackInPlace = false;
    [HideInInspector]
    public bool AreAllFeetInPlace = false;

    // Start is called before the first frame update
    void Start()
    {
        LeftFrontCol = Col_LeftFrontArmPlace.GetComponent<BoxCollider>();
        LeftBackCol = Col_LeftBackArmPlace.GetComponent<BoxCollider>();
        RightFrontCol = Col_RightFrontArmPlace.GetComponent<BoxCollider>();
        RightBackCol = Col_RightBackArmPlace.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
