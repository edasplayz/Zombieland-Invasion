using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Device;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] GameObject titleScreenManinMenu;
    [SerializeField] GameObject titleScreenLoadMenu;

    [Header("Buttons")]
    [SerializeField] Button LoadMenuReturnButton;
    [SerializeField] Button mainMenuLoadGameButton;
    public void StartNetworkAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }
    public void StartNewGame()
    {
        WorldSaveGameManager.instance.CreateNewGame();
        StartCoroutine(WorldSaveGameManager.instance.LoadWorldScene());
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
}
