using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance; //References to current instance.

    void Awake()
    {
        //Allows only one to run.
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
            return;
        }

        //DDoL so we don't haev to keep instantiating.
        DontDestroyOnLoad(gameObject);

        //Set values for every Sound in our array.
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
    }

    //Play a sound dependant on its name. => public call method called using "FindObjectOfType<AudioManager>().PlaySound("name");"

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("[AudioManager.cs] Sound: <" + name + "> not found!");
            return;
        }

        s.source.Play();
    }

    //Public sound calls.
    #region Sounds
    public void UISound_Forward()
    {
        FindObjectOfType<AudioManager>().PlaySound("UI_Select_Forward");
    }

    public void UISound_Backward()
    {
        FindObjectOfType<AudioManager>().PlaySound("UI_Select_Backward");
    }

    public void UISound_Normal()
    {
        FindObjectOfType<AudioManager>().PlaySound("UI_Select_Normal");
    }
    #endregion
}
