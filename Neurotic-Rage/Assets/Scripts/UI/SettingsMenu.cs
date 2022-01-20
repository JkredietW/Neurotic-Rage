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
    public TMP_Dropdown resDrop;
    public Slider masterslider;
    public Slider uislider;
    public Slider sfxslider;
    public Slider musicslider;

    private void Awake()
    {
        mixer.SetFloat("Master", PlayerPrefs.GetFloat("Master", 0));
        mixer.SetFloat("Music", PlayerPrefs.GetFloat("Music", 0));
        mixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFX", 0));
        mixer.SetFloat("UI", PlayerPrefs.GetFloat("UI", 0));

        masterslider.value = PlayerPrefs.GetFloat("Master", 0);
        musicslider.value = PlayerPrefs.GetFloat("Music", 0);
        sfxslider.value = PlayerPrefs.GetFloat("SFX", 0);
        uislider.value = PlayerPrefs.GetFloat("UI", 0);
    }
    private void Start()
    {
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
    public void SetMainVolume(Slider sliderValue)
    {
        mixer.SetFloat("Master", sliderValue.value);
        PlayerPrefs.SetFloat("Master", sliderValue.value);
    }
    public void SetMusicVolume(Slider sliderValue)
    {
        mixer.SetFloat("Music", sliderValue.value);
        PlayerPrefs.SetFloat("Music", sliderValue.value);
    }
    public void SetSfxVolume(Slider sliderValue)
    {
        mixer.SetFloat("SFX", sliderValue.value);
        PlayerPrefs.SetFloat("SFX", sliderValue.value);
    }
    public void SetUiVolume(Slider sliderValue)
    {
        mixer.SetFloat("UI", sliderValue.value);
        PlayerPrefs.SetFloat("UI", sliderValue.value);
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
