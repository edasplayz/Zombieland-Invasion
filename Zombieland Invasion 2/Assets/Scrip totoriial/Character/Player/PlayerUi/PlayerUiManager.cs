using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerUiManager : MonoBehaviour
{
    public static PlayerUiManager instance;

    [Header("Network Join")]
    [SerializeField] bool startGameAsClient;

    [HideInInspector] public PlayerUIHudManager playerUIHudManager;
    [HideInInspector] public PlayerUiPopUpManager playerUiPopUpManager;

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
}
