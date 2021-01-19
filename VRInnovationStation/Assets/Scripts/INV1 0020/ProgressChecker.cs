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

    public void IncreasePercentageBy(float number)
    {
        PercentageComplete = PercentageComplete + number;
        Debug.Log("<color=cyan>[ProgressChecker.cs]</color>Percentage complete has been changed by: <" + number + "%>. Overall percentage is now: <" + PercentageComplete + "%>.");
        PercentageDisplayText.text = PercentageComplete + "%";
        FindObjectOfType<AudioManager>().PlaySound("UI_PercentUp");
    }

    public void DecreasePercentageBy(float number)
    {
        PercentageComplete = PercentageComplete - number;
        Debug.Log("<color=cyan>[ProgressChecker.cs]</color>Percentage complete has been changed by: <-" + number + "%>. Overall percentage is now: <" + PercentageComplete + "%>.");
        PercentageDisplayText.text = PercentageComplete + "%";
        FindObjectOfType<AudioManager>().PlaySound("UI_PercentDown");
    }

    public void ChangePercentageTo(float number)
    {
        PercentageComplete = number;
        Debug.Log("<color=cyan>[ProgressChecker.cs]</color>Percentage complete has been force changed to: <" + number + "%>.");
        PercentageDisplayText.text = PercentageComplete + "%";
        FindObjectOfType<AudioManager>().PlaySound("UI_PercentUp");
    }
}
