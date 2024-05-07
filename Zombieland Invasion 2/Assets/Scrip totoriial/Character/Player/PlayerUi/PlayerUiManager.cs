using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Loading;

public class PlayerUiManager : MonoBehaviour
{
    public static PlayerUiManager instance;

    [SerializeField] GameObject loadingScreen;

    [Header("Network Join")]
    [SerializeField] bool startGameAsClient;

    [HideInInspector] public PlayerUIHudManager playerUIHudManager;
    [HideInInspector] public PlayerUiPopUpManager playerUiPopUpManager;

    [Header("UI Flags")]
    public bool menuWindowIsOpen = false; // inventory sceen equipment menu blacksmith menu ect
    public bool popUpWindowIsOpen = false; // item pick up dialogues pop up ect

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
        playerUiPopUpManager = GetComponentInChildren<PlayerUiPopUpManager>();

    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            startGameAsClient = true;
        }

        if (startGameAsClient)
        {
            startGameAsClient = false;
            // we must first shut down, becouse we have started as a host during the title screen
            NetworkManager.Singleton.Shutdown();
            // we then restart, as a client
            NetworkManager.Singleton.StartClient();
        }
    }

    public void EnableLoadingGameScreen()
    {
        loadingScreen.SetActive(true);
    }

    public void DisableLoadingGameScreen()
    {
        loadingScreen.SetActive(false);
    }


}
