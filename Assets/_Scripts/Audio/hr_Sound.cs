﻿using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class hr_Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1;
    [Range(0.1f, 3f)] public float pitch = 1;
    public bool loop;
    public AudioSource source;
    public AudioMixerGroup audioMixerGroup;
}
