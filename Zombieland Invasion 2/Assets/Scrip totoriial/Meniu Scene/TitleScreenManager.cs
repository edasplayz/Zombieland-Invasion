using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Device;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    

    public static TitleScreenManager Instance;
    [Header("Menus")]
    [SerializeField] GameObject titleScreenManinMenu;
    [SerializeField] GameObject titleScreenLoadMenu;

    [Header("Buttons")]
    [SerializeField] Button LoadMenuReturnButton;
    [SerializeField] Button mainMenuLoadGameButton;
    [SerializeField] Button mainMenuNewGameButton;
    [SerializeField] Button deleteCharacterPopUpConfirmButton;

    [Header("Pop Ups")]
    [SerializeField] GameObject noCharacterSlotsPopUp;
    [SerializeField] Button noCharacterSlotsOkayButton;
    [SerializeField] GameObject deleteCharacterSlotPopUp;

    [Header("Player UI")]
    public GameObject playerUi;

    [Header("Character Slots")]
    public CharacterSlot currentSelecterSlot = CharacterSlot.NO_SLOT;
    

    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void StartNetworkAsHost()
    {
        NetworkManager.Singleton.StartHost();

        
    }
    public void StartNewGame()
    {
        WorldSaveGameManager.instance.AttemptToCreateNewGame();

        PlayerCamera.instance.SetCursorLockState(true);

        
    }

    public void OpenLoadGameMenu()
    {
        // close main meniu
        titleScreenManinMenu.SetActive(false);
        // open load menu
        titleScreenLoadMenu.SetActive(true);

        // select the return button first
        LoadMenuReturnButton.Select();

        
    }

    public void CloseLoadGameMenu()
    {
        // open load menu
        titleScreenLoadMenu.SetActive(false);
        // close main meniu
        titleScreenManinMenu.SetActive(true);
        
        

        // select the load button
        mainMenuLoadGameButton.Select();
    }

    public void DisplayNoFreeCharacterSlotsPopUp()
    {
        noCharacterSlotsPopUp.SetActive(true);
        noCharacterSlotsOkayButton.Select();
    }

    public void CloseNoFreeCharacterSlotsPopUp()
    {
        noCharacterSlotsPopUp.SetActive(false);
        mainMenuNewGameButton.Select();
    }

    // character slots

    public void SelectCharacterSlot(CharacterSlot characterSlot)
    {
        currentSelecterSlot = characterSlot;
    }

    public void SelectNoSlot()
    {
        currentSelecterSlot = CharacterSlot.NO_SLOT;
    }

    public void AttemptToDeleteCharacterSlot()
    {
        if(currentSelecterSlot != CharacterSlot.NO_SLOT)
        {
            deleteCharacterSlotPopUp.SetActive(true);
            deleteCharacterPopUpConfirmButton.Select();
        }
        
    }

    public void DeleteCharacterSlot()
    {
        deleteCharacterSlotPopUp.SetActive(false);
        WorldSaveGameManager.instance.DeleteGame(currentSelecterSlot);

        // we disable ant thern enable to refresh the slots (the deleted slots will now become inactive)
        titleScreenLoadMenu.SetActive(false );
        titleScreenLoadMenu.SetActive(true );
        LoadMenuReturnButton.Select();
        
        
    }

    public void CloseDeleteCharacterPopUp()
    {
        deleteCharacterSlotPopUp.SetActive(false );
        LoadMenuReturnButton.Select();
    }
}
