using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_VersionNumber : MonoBehaviour
{
    public TextMeshProUGUI VersionText;

    // Start is called before the first frame update
    void Start()
    {
        VersionText = this.GetComponent<TextMeshProUGUI>();
        VersionText.text = ("Build <" + Application.version + ">");
    }
}
