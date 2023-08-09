using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    public Slider musicSlider;
    public Slider soundSlider;
    private float musicValueForSlider;
    private float soundValueForSlider;
    public bool isMainMenu = false;

    private LevelManager levelManager;
    public void Start()
    {
        if(!isMainMenu) levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        InitVolumeAndSound();

        // resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        // resolutionDropdown.ClearOptions();

        // List<string> options = new List<string>();
        // int currentResolutionIndex = 0;
        // for (int i = 0; i < resolutions.Length; i++)
        // {
        //     string option = resolutions[i].width + "x" + resolutions[i].height;
        //     options.Add(option);

        //     if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
        //     {
        //         currentResolutionIndex = i;
        //     }
        // }

        // resolutionDropdown.AddOptions(options);
        // resolutionDropdown.value = currentResolutionIndex;
        // resolutionDropdown.RefreshShownValue();

        // Screen.fullScreen = true;
    }
    private void InitVolumeAndSound()
    {
        musicSlider.value = PlayerPrefs.GetFloat("Music", -20);
        audioMixer.SetFloat("Music", musicSlider.value);

        soundSlider.value = PlayerPrefs.GetFloat("Sound", -20);
        audioMixer.SetFloat("Sound", soundSlider.value);
    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("Music", volume);
        audioMixer.SetFloat("Music", volume);
    }

    public void SetSoundVolume(float volume)
    {
        PlayerPrefs.SetFloat("Sound", volume);
        audioMixer.SetFloat("Sound", volume);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ClearSavedData()
    {
        PlayerPrefs.DeleteAll();
    }
    public void CloseSettingsWindow()
    {
        levelManager.CloseSettingsWindow();
    }
}