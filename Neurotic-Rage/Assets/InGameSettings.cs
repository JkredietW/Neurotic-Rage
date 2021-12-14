using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class InGameSettings : MonoBehaviour
{
    public AudioMixer mixer;
    public Resolution[] resolutions;
    public int resInt;
    public TMP_Dropdown resDrop;
    public Light brightnisLight;
    public Slider brightnissslider;
    public Slider masterslider;
    public Slider uislider;
    public Slider sfxslider;
    public Slider musicslider;
    private void Start()
    {
        mixer.SetFloat("Master", Mathf.Log10(PlayerPrefs.GetFloat("Master")) * 20);
        mixer.SetFloat("Music", Mathf.Log10(PlayerPrefs.GetFloat("Music")) * 20);
        mixer.SetFloat("SFX", Mathf.Log10(PlayerPrefs.GetFloat("SFX")) * 20);
        mixer.SetFloat("UI", Mathf.Log10(PlayerPrefs.GetFloat("UI")) * 20);

        masterslider.value = PlayerPrefs.GetFloat("Master");
        musicslider.value = PlayerPrefs.GetFloat("Music");
        sfxslider.value = PlayerPrefs.GetFloat("SFX");
        uislider.value = PlayerPrefs.GetFloat("UI");

        brightnissslider.value = PlayerPrefs.GetFloat("Bright")*1000;//0.00 - 0.1
        brightnisLight.intensity = PlayerPrefs.GetFloat("Bright");

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
        mixer.SetFloat("Master", Mathf.Log10(sliderValue.value) * 20);
        PlayerPrefs.SetFloat("Master", sliderValue.value);
    }
    public void SetBrightness(Slider sliderValue)
    {
        brightnisLight.intensity = sliderValue.value / 1000;
        PlayerPrefs.SetFloat("Bright", sliderValue.value / 1000);
    }
    public void SetMusicVolume(Slider sliderValue)
    {
        mixer.SetFloat("Music", Mathf.Log10(sliderValue.value) * 20);
        PlayerPrefs.SetFloat("Music", sliderValue.value);
    }
    public void SetSfxVolume(Slider sliderValue)
    {
        mixer.SetFloat("SFX", Mathf.Log10(sliderValue.value) * 20);
        PlayerPrefs.SetFloat("SFX", sliderValue.value);
    }
    public void SetUiVolume(Slider sliderValue)
    {
        mixer.SetFloat("UI", Mathf.Log10(sliderValue.value) * 20);
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