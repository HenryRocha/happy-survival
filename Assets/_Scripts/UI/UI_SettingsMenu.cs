using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UI_SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private float masterVol, bgmVol, sfxVol;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        audioMixer.GetFloat("master_volume", out masterVol);
        masterVolumeSlider.value = masterVol;

        audioMixer.GetFloat("bgm_volume", out bgmVol);
        bgmVolumeSlider.value = bgmVol;

        audioMixer.GetFloat("sfx_volume", out sfxVol);
        sfxVolumeSlider.value = sfxVol;
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("master_volume", volume);
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("bgm_volume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfx_volume", volume);
    }
}
