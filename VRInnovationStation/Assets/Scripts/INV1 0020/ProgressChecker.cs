using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProgressChecker : MonoBehaviour
{
    private static float PercentageComplete;

    public TextMeshProUGUI PercentageDisplayText;

    // Start is called before the first frame update
    void Start()
    {
        PercentageComplete = 0f;
        PercentageDisplayText.text = PercentageComplete + "%";
    }

    //Increase the percentage of the progress system by the designated number, play a little SFX.
    public void IncreasePercentageBy(float number)
    {
        //Prevent overflow.
        if (PercentageComplete < 100f)
        {
            PercentageComplete = PercentageComplete + number;
            Debug.Log("<color=cyan>[ProgressChecker.cs]</color> Percentage complete has been changed by: <" + number + "%>. Overall percentage is now: <" + PercentageComplete + "%>.");
            PercentageDisplayText.text = PercentageComplete + "%";
            FindObjectOfType<AudioManager>().PlaySound("UI_PercentUp");

            if (PercentageComplete > 100f)
            {
                PercentageComplete = 100f;
            }
        }
    }

    //Decrease the percentage of the progress system by the designated number, play a little SFX.
    public void DecreasePercentageBy(float number)
    {
        //Prevent underflow.
        if (PercentageComplete >= 0f)
        {
            PercentageComplete = PercentageComplete - number;
            Debug.Log("<color=cyan>[ProgressChecker.cs]</color> Percentage complete has been changed by: <-" + number + "%>. Overall percentage is now: <" + PercentageComplete + "%>.");
            PercentageDisplayText.text = PercentageComplete + "%";
            FindObjectOfType<AudioManager>().PlaySound("UI_PercentDown");

            if (PercentageComplete < 0f)
            {
                PercentageComplete = 0f;
            }
        }
    }

    //Force set the percentage of the progress system by the designated number, play a little SFX.
    public void ChangePercentageTo(float number)
    {
        //Prevent under/overflow.
        if (PercentageComplete >= 0f || PercentageComplete < 100f)
        {
            PercentageComplete = number;
            Debug.Log("<color=cyan>[ProgressChecker.cs]</color> Percentage complete has been force changed to: <" + number + "%>.");
            PercentageDisplayText.text = PercentageComplete + "%";
            FindObjectOfType<AudioManager>().PlaySound("UI_PercentUp");

            if (PercentageComplete < 0f)
            {
                PercentageComplete = 0f;
            }

            else if (PercentageComplete > 100f)
            {
                PercentageComplete = 100f;
            }
        }
    }
}
