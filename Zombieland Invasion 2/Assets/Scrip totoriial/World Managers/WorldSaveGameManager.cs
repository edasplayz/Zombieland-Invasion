using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager instance;

    [SerializeField] private PlayerManager player;

    [Header("SAVE/LOAD")]
    [SerializeField] bool saveGame;
    [SerializeField] bool loadGame;

    [Header("World Scene Index")]
    [SerializeField] int worldSceneIndex = 1;

    [Header("Save Data Writer")]
    private SaveFileDataWriter saveFileDataWriter;

    [Header("Current Character Data")]
    public CharacterSlot currentCharacterSlotBeingUsed;
    public CharacterSaveData currentCharacterData;
    private string saveFileName;

    [Header("Character Slots")]
    public CharacterSaveData characterSlot01;
    public CharacterSaveData characterSlot02;
    public CharacterSaveData characterSlot03;
    public CharacterSaveData characterSlot04;
    public CharacterSaveData characterSlot05;
    public CharacterSaveData characterSlot06;
    public CharacterSaveData characterSlot07;
    public CharacterSaveData characterSlot08;
    public CharacterSaveData characterSlot09;
    public CharacterSaveData characterSlot10;

    private void Awake()
    {
        // there can only be one instance of this script at one time, if another exist, destroy it
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadAllCharacterProfiles();
    }

    private void Update()
    {
        if(saveGame)
        {
            saveGame = false;
            SaveGame();
        }

        if (loadGame)
        {
            loadGame = false;
            LoadGame();
        }
    }

    public string DecideCharacterFileNameBesedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)
    {
        string fileName = "";
        switch (characterSlot)
        {
            case CharacterSlot.CharacterSlot_01:
                fileName = "characterSlot_01";
                break; 
            case CharacterSlot.CharacterSlot_02:
                fileName = "characterSlot_02";
                break;
            case CharacterSlot.CharacterSlot_03:
                fileName = "characterSlot_03";
                break;
            case CharacterSlot.CharacterSlot_04:
                fileName = "characterSlot_04";
                break;
            case CharacterSlot.CharacterSlot_05:
                fileName = "characterSlot_05";
                break;
            case CharacterSlot.CharacterSlot_06:
                fileName = "characterSlot_06";
                break;
            case CharacterSlot.CharacterSlot_07:
                fileName = "characterSlot_07";
                break;
            case CharacterSlot.CharacterSlot_08:
                fileName = "characterSlot_08";
                break;
            case CharacterSlot.CharacterSlot_09:
                fileName = "characterSlot_09";
                break;
            case CharacterSlot.CharacterSlot_10:
                fileName = "characterSlot_10";
                break;
            default: 
                break;

        }
        return fileName;
    }

    public void CreateNewGame()
    {
        // create a new file with file name depending on which character slot we are using 
        saveFileName = DecideCharacterFileNameBesedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

        currentCharacterData = new CharacterSaveData();
    }

    public void LoadGame()
    {
        // load a previos file with a file name depending on which slot we are using
        saveFileName = DecideCharacterFileNameBesedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

        saveFileDataWriter = new SaveFileDataWriter();
        // generaly works on multiple machine types (application.persistentDataPath)
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;
        currentCharacterData = saveFileDataWriter.LoadSaveFile();

        StartCoroutine(LoadWorldScene());
    }

    public void SaveGame()
    {
        // save the current file under a file name depending on which slot we are using 
        saveFileName = DecideCharacterFileNameBesedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

        saveFileDataWriter = new SaveFileDataWriter();
        // generaly works on multiple machine types (application.persistentDataPath)
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;
        // pass the player info, from game, to their save file 
        player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);

        // write that info onto a json file, saved to this machine
        saveFileDataWriter.CreateNewCharaterSaveFile(currentCharacterData);

        
    }

    // load all character profiles on divice when starting game

    private void LoadAllCharacterProfiles()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBesedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
        characterSlot01 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBesedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
        characterSlot02 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBesedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
        characterSlot03 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBesedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
        characterSlot04 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBesedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
        characterSlot05 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBesedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
        characterSlot06 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBesedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
        characterSlot07 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBesedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
        characterSlot08 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBesedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
        characterSlot09 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBesedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
        characterSlot10 = saveFileDataWriter.LoadSaveFile();
    }
    public IEnumerator LoadWorldScene()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
        yield return null;
    }

    public int GetWorldSceneIndex()
    {
        return worldSceneIndex;
    }
}
