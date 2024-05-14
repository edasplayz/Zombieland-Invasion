using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public Slider verticalSensitivity;
    public Slider horizontalSensitivity;
    public Slider masterVolume;
    public Slider musicVolume;
    public Slider soundVolume;

    public AudioMixer audioMixer;

    public GameObject settingMenu;

    

    // PlayerPrefs keys for saving and loading volume settings
    private const string MasterVolumeKey = "MasterVolume";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SoundVolumeKey = "SoundVolume";

    private void Start()
    {
        if(PlayerPrefs.HasKey(MasterVolumeKey) || PlayerPrefs.HasKey(MusicVolumeKey) || PlayerPrefs.HasKey(SoundVolumeKey))
        {
            LoadVolume();
        }
        else
        {
            ChangeMasterMolume();
            ChangeMusicMolume();
            ChangeSoundMolume();
        }

        LoadSensitivitySettings();

    }

    public void EnableMenu()
    {
        if (settingMenu != null)
        {
            settingMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            
        }
        
    }

    public void DisableMenu()
    {
        if (settingMenu != null)
        {
            settingMenu.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
    }

    public void ChangeMasterMolume()
    {
        audioMixer.SetFloat("Master", (masterVolume.value));
        PlayerPrefs.SetFloat(MasterVolumeKey, masterVolume.value);
    }

    public void ChangeMusicMolume()
    {
        audioMixer.SetFloat("Music", musicVolume.value);
        PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume.value);
    }

    public void ChangeSoundMolume()
    {
        audioMixer.SetFloat("Sound", soundVolume.value);
        PlayerPrefs.SetFloat (SoundVolumeKey, soundVolume.value);
    }

    private void LoadVolume()
    {
        masterVolume.value = PlayerPrefs.GetFloat(MasterVolumeKey);
        musicVolume.value = PlayerPrefs.GetFloat(MusicVolumeKey);
        soundVolume.value = PlayerPrefs.GetFloat(SoundVolumeKey);

        ChangeMasterMolume();
        ChangeMusicMolume();
        ChangeSoundMolume();
    }

    public void ChangeVerticalSensitivity()
    {
        // Modify vertical sensitivity in PlayerCamera
        PlayerCamera.instance.ChangeVerticalSensitivity(verticalSensitivity.value);
        PlayerPrefs.SetFloat("VerticalSensitivity", verticalSensitivity.value);
    }

    public void ChangeHorizontalSensitivity()
    {
        // Modify horizontal sensitivity in PlayerCamera
        PlayerCamera.instance.ChangeHorizontalSensitivity(horizontalSensitivity.value);
        PlayerPrefs.SetFloat("HorizontalSensitivity", horizontalSensitivity.value);
    }

    private void LoadSensitivitySettings()
    {
        // Load sensitivity settings from PlayerPrefs and set slider values
        
            verticalSensitivity.value = PlayerPrefs.GetFloat("VerticalSensitivity");

            horizontalSensitivity.value = PlayerPrefs.GetFloat("HorizontalSensitivity");
            
            ChangeHorizontalSensitivity(); // Apply the sensitivity immediately
            ChangeVerticalSensitivity(); // Apply the sensitivity immediately


    }
}
