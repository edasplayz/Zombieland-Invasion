using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerNetworkManager playerNetworkManager;
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    protected override void Awake()
    {
        base.Awake();

        // do more stuff, only for the player

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    protected override void Update()
    {
        base.Update();

        // if we do not own this gameobject, we do not controll or edit it 
        if(!IsOwner) 
        {
            return;
        }
        // handle movement
        playerLocomotionManager.HandleAllMovement();

        //regen stamina
        playerStatsManager.RegenerateStamina();
    }

    protected override void LateUpdate()
    {

        if(!IsOwner) 
        { 
            return;
        }
        base.LateUpdate();

        PlayerCamera.instance.HandleAllCameraActions();
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // if this the player object owned by client
        if(IsOwner)
        {
            PlayerCamera.instance.player = this;
            PlayerInputManager.Instance.player = this;
            WorldSaveGameManager.instance.player = this;

            playerNetworkManager.currentStamina.OnValueChanged += PlayerUiManager.instance.playerUIHudManager.SetNewStaminaValue;
            playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;

            // this will be moved when saving and loading is added
            playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
            playerNetworkManager.currentStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
            PlayerUiManager.instance.playerUIHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
            

        }
    }

    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.zPosition = transform.position.z;
    }

    public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        playerNetworkManager.characterName.Value = currentCharacterData.characterName;
        Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
        transform.position = myPosition;
        
    }
}
