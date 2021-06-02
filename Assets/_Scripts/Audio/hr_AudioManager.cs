using System;
using UnityEngine;
using UnityEngine.Audio;

public class hr_AudioManager : MonoBehaviour
{
    public static hr_AudioManager instance = null;

    [SerializeField] private hr_Sound[] sounds;
    [SerializeField] private hr_Sound[] scaredShoutsSounds;

    private float scaredShoutLastPlayed = -5.0f;
    private float scaredShoutTimeDiff = 5.0f;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            foreach (hr_Sound sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
                sound.source.outputAudioMixerGroup = sound.audioMixerGroup;
            }

            foreach (hr_Sound sound in scaredShoutsSounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
                sound.source.outputAudioMixerGroup = sound.audioMixerGroup;
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        Play("bgm");
    }

    /// <summary>
    /// Plays a sound by the given name.
    /// </summary>
    public void Play(string name)
    {
        hr_Sound sound = Array.Find(sounds, s => s.name == name);

        if (sound != null)
        {
            sound.source.Play();
        }
        else
        {
            Debug.LogWarning($"Could not find audio with name '{name}'");
        }
    }

    public void PlayScaredShout()
    {
        if (Time.time - scaredShoutLastPlayed > scaredShoutTimeDiff)
        {
            int shout = UnityEngine.Random.Range(0, scaredShoutsSounds.Length);
            hr_Sound sound = scaredShoutsSounds[shout];

            sound.source.Play();
            scaredShoutLastPlayed = Time.time;

        }
    }

    public void ResetAndStop(string name)
    {
        hr_Sound sound = Array.Find(sounds, s => s.name == name);

        if (sound != null)
        {
            sound.source.Stop();
            sound.source.time = 0f;
        }

        foreach (hr_Sound s in sounds)
        {
            Debug.Log($"hr_Sound name: {s.name}");
        }
    }

    public void SetVolume(string name, float volume)
    {
        hr_Sound sound = Array.Find(sounds, s => s.name == name);

        if (sound != null)
        {
            sound.source.volume = volume;
        }
    }
}
