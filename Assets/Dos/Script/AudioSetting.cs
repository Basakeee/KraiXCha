using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
[System.Serializable]
public class AudioSetting : MonoBehaviour
{
    public static AudioSetting Instance;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer; // Drag your Audio Mixer here.

    [Header("UI Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    [Header("All Audio Clip")]
    public AllsouceSFX[] Allsouce;
    private Dictionary<string, AudioClip> Sound;
    private float masterVolume;
    private float musicVolume;
    private float sfxVolume;

    private void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

        Sound = new Dictionary<string, AudioClip>();
        audioMixer.GetFloat("Master", out masterVolume);
        audioMixer.GetFloat("BGM", out musicVolume);
        audioMixer.GetFloat("SFX", out sfxVolume);

        // Normalize the volume values to be in range of 0 to 1 for sliders
        masterSlider.value = Mathf.Pow(10, masterVolume / 20); // Convert dB to linear
        musicSlider.value = Mathf.Pow(10, musicVolume / 20); // Convert dB to linear
        sfxSlider.value = Mathf.Pow(10, sfxVolume / 20); // Convert dB to linear

        // Add listeners to sliders
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        //Add sound to dict
        foreach (AllsouceSFX _sound in Allsouce)
        {
            Sound.Add(_sound.name, _sound.clip);
        }
        PlayMusic("BGM");
    }

    public void UpdateSlider()
    {
        masterSlider = GameObject.Find("MasterSlider").GetComponent<Slider>();
        musicSlider = GameObject.Find("BGMslider").GetComponent<Slider>();
        sfxSlider = GameObject.Find("EffectSlider").GetComponent<Slider>();
        // Get current volume levels from the Audio Mixer
        audioMixer.GetFloat("Master", out masterVolume);
        audioMixer.GetFloat("BGM", out musicVolume);
        audioMixer.GetFloat("SFX", out sfxVolume);

        // Normalize the volume values to be in range of 0 to 1 for sliders
        masterSlider.value = Mathf.Pow(10, masterVolume / 20); // Convert dB to linear
        musicSlider.value = Mathf.Pow(10, musicVolume / 20); // Convert dB to linear
        sfxSlider.value = Mathf.Pow(10, sfxVolume / 20); // Convert dB to linear
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void PlayMusic(string name)
    {
        if (Sound.TryGetValue(name, out AudioClip clip))
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }

    }

    public void PlaySFX(string name)
    {
        if (Sound.TryGetValue(name, out AudioClip clip))
            sfxSource.PlayOneShot(clip);
    }

    public void SetMusicVolume(float volume)
    {
        // Convert linear volume to dB and apply it to the audio mixer
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        // Convert linear volume to dB and apply it to the audio mixer
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    public void SetMasterVolume(float volume)
    {
        // Convert linear volume to dB and apply it to the audio mixer
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }
}


[System.Serializable]
public class AllsouceSFX
{
    public AudioClip clip;
    public string name;
}