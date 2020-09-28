using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Voice.PUN;

public class HighlightVoice : MonoBehaviour
{
    [SerializeField]
    private Image micImage;

    [SerializeField]
    private Image speakerImage;

    [SerializeField]
    private PhotonVoiceView photonVoiceView;


    private void Awake()
    {

        //micImage = GetComponentInParent<VoiceDebugUI>().transform.Find("Image_Mic").GetComponent<Image>();
        this.micImage.enabled = false;
        this.speakerImage.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        this.micImage.enabled = this.photonVoiceView.IsRecording;
        this.speakerImage.enabled = this.photonVoiceView.IsSpeaking;
    }
}
