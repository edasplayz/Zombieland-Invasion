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
    public Button resetButton;
    public Button quitButton;

    public AudioMixer audioMixer;

    public GameObject settingMenu;

    public WorldSaveGameManager worldSaveGameManager;
    public PlayerManager playerManager;

    [Header("Test Buttons")]
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button button6;


    // PlayerPrefs keys for saving and loading volume settings
    private const string MasterVolumeKey = "MasterVolume";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SoundVolumeKey = "SoundVolume";

    private GameObject playerObject; // Store the found player object

    private void Awake()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }
        
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey(MasterVolumeKey) || PlayerPrefs.HasKey(MusicVolumeKey) || PlayerPrefs.HasKey(SoundVolumeKey))
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
        PlayerPrefs.SetFloat(SoundVolumeKey, soundVolume.value);
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

    public void RestartGame()
    {
        WorldAIManager.instance.DespawnAllCharacters();
        worldSaveGameManager.LoadGame();

        //playerManager.RevivePlayer();


    }

    public void Button1Clicked()
    {
        
        Vector3 desiredCoordinates = new Vector3(37.5837097f, 2.44000006f, 35.0079155f);
        MovePlayerToSpecificCoordinates(desiredCoordinates);
    }

    public void Button2Clicked()
    {
        
        Vector3 desiredCoordinates = new Vector3(-35.1300011f, 3.24000001f, 34.6039886f);
        MovePlayerToSpecificCoordinates(desiredCoordinates);
    }

    public void Button3Clicked()
    {
        
        Vector3 desiredCoordinates = new Vector3(3.30910707f, 3.74000001f, 74.7075272f);
        MovePlayerToSpecificCoordinates(desiredCoordinates);
    }

    public void Button4Clicked()
    {
        
        Vector3 desiredCoordinates = new Vector3(58.1899986f, 4.21999979f, 76.5557022f);
        MovePlayerToSpecificCoordinates(desiredCoordinates);
    }
    public void Button5Clicked()
    {
        
        Vector3 desiredCoordinates = new Vector3(-28.338604f, 4.95730782f, 76.7557907f);
        MovePlayerToSpecificCoordinates(desiredCoordinates);
    }
    public void Button6Clicked()
    {
        
        Vector3 desiredCoordinates = new Vector3(-37.5666199f, 3.54999995f, 118.253075f);
        MovePlayerToSpecificCoordinates(desiredCoordinates);
    }

    public void MovePlayerToSpecificCoordinates(Vector3 coordinates) 
    {
        if (playerObject != null)
        {
            playerObject.transform.position = coordinates; // Move the player to the specified coordinates
        }
        else
        {
            Debug.LogError("Player object not found! Cannot move player.");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
