using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class PlayerManager : CharacterManager
{
    /*
    [Header("Debug Menu")]
    [SerializeField] bool respawnCharacter = false;
    [SerializeField] bool switchRightWeapon = false;
    */

    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerNetworkManager playerNetworkManager;
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;
    [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
    [HideInInspector] public PlayerCombatManager playerCombatManager;
    [HideInInspector] public PlayerInteractionManager playerInteractionManager;
    public MenuManager menuManager;
    protected override void Awake()
    {
        base.Awake();

        // do more stuff, only for the player
        menuManager = FindObjectOfType<MenuManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        playerInteractionManager = GetComponent<PlayerInteractionManager>();
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

        //DebugMenu();

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

    protected override void OnEnable()
    {
        base.OnEnable();

        
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        //transform.position = new Vector3(0f, 4f, -7.948008f);
       // transform.rotation = Quaternion.Euler(0, -0.423f, 0);

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallBack;

        // if this the player object owned by client
        if(IsOwner)
        {
            PlayerCamera.instance.player = this;
            PlayerInputManager.Instance.player = this;
            WorldSaveGameManager.instance.player = this;

            // update the total amount of health or stamina when the stat linked to either changes
            playerNetworkManager.vitality.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
            playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;
            // updates ui stat bar when a stat changes (health or stamina)
            playerNetworkManager.currentHealth.OnValueChanged += PlayerUiManager.instance.playerUIHudManager.SetNewHealthValue;
            playerNetworkManager.currentStamina.OnValueChanged += PlayerUiManager.instance.playerUIHudManager.SetNewStaminaValue;
            playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;

            
            

        }

        // only opdate floating hp bar if this character is not the local player character (you dont wanna see a hp bar floating above your own head)
        if (!IsOwner)
        {
            characterNetworkManager.currentHealth.OnValueChanged += characterUIManager.OnHpChange;
        }

        //stats
        playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHP;

        // lock on 
        playerNetworkManager.isLockedOn.OnValueChanged += playerNetworkManager.OnIsLockedOnChange;
        playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged += playerNetworkManager.OnLockOnTargetIDChange;

        // equipment
        playerNetworkManager.currentRightHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentRightHandWeaponIDChange;
        playerNetworkManager.currentLeftHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
        playerNetworkManager.currentWeaponBeingUsed.OnValueChanged += playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;
        playerNetworkManager.isBlocking.OnValueChanged += playerNetworkManager.OnIsBlockingChnage;

        // flags
        playerNetworkManager.isChargingAttack.OnValueChanged += playerNetworkManager.OnIsChargingAttackCharge;

        // upon connecting if we are the owner of this character but we are not the server reload our character data to this newly instatiated character
        // we dont run this if we are the server becouse since they are the host they are already loaded in and dont need to reload theyr data
        if (IsOwner && !IsServer)
        {
            LoadGameDataFromCurrentCharacterData(ref WorldSaveGameManager.instance.currentCharacterData);
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallBack;

        // if this the player object owned by client
        if (IsOwner)
        {

            // update the total amount of health or stamina when the stat linked to either changes
            playerNetworkManager.vitality.OnValueChanged -= playerNetworkManager.SetNewMaxHealthValue;
            playerNetworkManager.endurance.OnValueChanged -= playerNetworkManager.SetNewMaxStaminaValue;
            // updates ui stat bar when a stat changes (health or stamina)
            playerNetworkManager.currentHealth.OnValueChanged -= PlayerUiManager.instance.playerUIHudManager.SetNewHealthValue;
            playerNetworkManager.currentStamina.OnValueChanged -= PlayerUiManager.instance.playerUIHudManager.SetNewStaminaValue;
            playerNetworkManager.currentStamina.OnValueChanged -= playerStatsManager.ResetStaminaRegenTimer;

        }
        if (!IsOwner)
        {
            characterNetworkManager.currentHealth.OnValueChanged -= characterUIManager.OnHpChange;
        }

        //stats
        playerNetworkManager.currentHealth.OnValueChanged -= playerNetworkManager.CheckHP;

        // lock on 
        playerNetworkManager.isLockedOn.OnValueChanged -= playerNetworkManager.OnIsLockedOnChange;
        playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged -= playerNetworkManager.OnLockOnTargetIDChange;

        // equipment
        playerNetworkManager.currentRightHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentRightHandWeaponIDChange;
        playerNetworkManager.currentLeftHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
        playerNetworkManager.currentWeaponBeingUsed.OnValueChanged -= playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;
        playerNetworkManager.isBlocking.OnValueChanged -= playerNetworkManager.OnIsBlockingChnage;

        // flags
        playerNetworkManager.isChargingAttack.OnValueChanged -= playerNetworkManager.OnIsChargingAttackCharge;

    }

    private void OnClientConnectedCallBack(ulong clientID)
    {
        // keep a list of active players in the game
        WorldGameSessionManager.Instance.AddPlayerToActivePlayersList(this);
        
        // if we are the server we are the host so we dont need to load players to sync them
        // you only need to load other players gear to sync it if you join a game thats already been active without you being present
        if(!IsServer && IsOwner)
        {
            foreach (var player in WorldGameSessionManager.Instance.players)
            {
                if (player != this)
                {
                    player.LoadOtherPlayerCharacterWhenJoiningServer();
                }
            }
        }
    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {

        if (IsOwner)
        {
            PlayerUiManager.instance.playerUiPopUpManager.SendYouDiedPopUp();
        }


        if (IsOwner)
        {
            characterNetworkManager.currentHealth.Value = 0;
            isDead.Value = true;

            // reset any flags here that need to be reset 
            // nothing yet

            // if we are not grounded, play an aerial death animation

            if (!manuallySelectDeathAnimation)
            {
                characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
            }
        }

        

        // Wait for a period of time
        yield return new WaitForSeconds(5);

        Debug.Log("Jis numire");
        //return base.ProcessDeathEvent(manuallySelectDeathAnimation);
        menuManager.RestartGame();
        ReviveCharacter();


        // check for players that are alime, if 0 respawn character


    }

    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.zPosition = transform.position.z;

        currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
        currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value;

        currentCharacterData.vitality = playerNetworkManager.vitality.Value;
        currentCharacterData.endurance = playerNetworkManager.endurance.Value;


    }

    public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        playerNetworkManager.characterName.Value = currentCharacterData.characterName;
        Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
        transform.position = myPosition;

        playerNetworkManager.vitality.Value = currentCharacterData.vitality;
        playerNetworkManager.endurance.Value = currentCharacterData.endurance;
        // this will be moved when saving and loading is added
        playerNetworkManager.maxHealth.Value = playerStatsManager.CalculateHealthBasedOnVitalityLevel(playerNetworkManager.vitality.Value);
        playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
        playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
        playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
        PlayerUiManager.instance.playerUIHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);

    }

    public void LoadOtherPlayerCharacterWhenJoiningServer()
    {
        // sync weapon
        playerNetworkManager.OnCurrentRightHandWeaponIDChange(0, playerNetworkManager.currentRightHandWeaponID.Value);
        playerNetworkManager.OnCurrentLeftHandWeaponIDChange(0, playerNetworkManager.currentLeftHandWeaponID.Value);

        // sync block status
        playerNetworkManager.OnIsBlockingChnage(false, playerNetworkManager.isBlocking.Value);

        // armor

        // lock on 
        if (playerNetworkManager.isLockedOn.Value)
        {
            playerNetworkManager.OnLockOnTargetIDChange(0, playerNetworkManager.currentTargetNetworkObjectID.Value);
        }
    }

    public void RevivePlayer()
    {

        ReviveCharacter();
        Debug.Log("Revive");


    }

    public override void ReviveCharacter()
    {
        base.ReviveCharacter();
        
        if (IsOwner)
        {
            isDead.Value = false;
            playerNetworkManager.currentHealth.Value = playerNetworkManager.maxHealth.Value;
            playerNetworkManager.currentStamina.Value = playerNetworkManager.maxStamina.Value;
            // restore focus points

            // play rebirth effects 
            playerAnimatorManager.PlayTargetActionAnimation("Empty", false);
        }
    }

    

    // debug delete later
    /*private void DebugMenu()
    {
        if (respawnCharacter)
        {
            respawnCharacter = false;
            ReviveCharacter();
        }
        if (switchRightWeapon)
        {
            switchRightWeapon = false;
            playerEquipmentManager.SwitchRightWeapon();
        }
    }*/
}
