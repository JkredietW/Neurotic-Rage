using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mixer;
    public Resolution[] resolutions;
    public int resInt;
    public TMP_Dropdown resDrop;
    private void Start()
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("masterVol")) * 20);
        mixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("musicVol")) * 20);
        mixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("sfxVol")) * 20);
        resolutions = Screen.resolutions;
        resDrop.ClearOptions();
        List<string> options = new List<string>();
        int currentResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }
        resDrop.AddOptions(options);
        resDrop.value = currentResIndex;
        resDrop.RefreshShownValue();
    }

    void Update()
    {
        
    }

    public void SetMainVolume(float sliderValue)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }
    public void SetMusicVolume(float sliderValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }
    public void SetSfxVolume(float sliderValue)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
    }
    public void SetUiVolume(float sliderValue)
    {
        mixer.SetFloat("UiVolume", Mathf.Log10(sliderValue) * 20);
    }
    public void SetResolution(int resIndex)
    {
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void FullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }
}
