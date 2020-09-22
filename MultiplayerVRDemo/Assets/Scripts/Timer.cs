using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private float theTimer;
    public float timerSpeed = .7f; //.7f seems to be the closest to real time
    private bool timerStarted;

    private static float endTime;

    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        timerStart();
    }

    // Update is called once per frame
    void Update()
    {
        //Start the timer...
        if (timerStarted)
        {
            theTimer += Time.fixedDeltaTime * timerSpeed;
            string hours = Mathf.Floor((theTimer % 260000) / 3600).ToString("00");
            string minutes = Mathf.Floor((theTimer % 3600) / 60).ToString("00");
            string seconds = (theTimer % 60).ToString("00"); //% calculates remainder

            //Set the text output.
            timerText.text = hours + ":" + minutes + ":" + seconds;
        }
    }

    //If the timer is started, reset to 0 and start.
    public void timerStart()
    {
        timerStarted = true;
        theTimer = 0;
    }

    //If the timer is stopped.
    public void timerEnd()
    {
        timerStarted = false;
        endTime = theTimer;
    }
}
