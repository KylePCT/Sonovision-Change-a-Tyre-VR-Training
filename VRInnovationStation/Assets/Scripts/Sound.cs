using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    //Sound values for editing in the inspector.

    public string name;
    public AudioClip clip;

    //Range allows the value to be treated as a slider with a Min and Max.
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(.1f, 3f)]
    public float pitch = 1f;

    public bool loop;
    public bool playOnAwake;

    [HideInInspector]
    public AudioSource source;
}
