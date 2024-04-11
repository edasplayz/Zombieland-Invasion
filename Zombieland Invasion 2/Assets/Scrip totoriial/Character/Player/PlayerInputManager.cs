using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;
    public PlayerManager player;

    PlayerControls playerControls;

    [Header("Movement input")]
    [SerializeField] Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    [Header("Camera movement input")]
    [SerializeField] Vector2 cameraInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;

    [Header("Lock On Input")]
    [SerializeField] bool lockOn_Input;

    [Header("Player action input")]
    [SerializeField] bool dodgeInput = false;
    [SerializeField] bool sprintInput = false;
    [SerializeField] bool jumpInput = false;
    [SerializeField] bool RB_Input = false;
    

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

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        // when the scene changes, run this logic
        SceneManager.activeSceneChanged += OnScreneChange;
        Instance.enabled = false;

        if (playerControls != null)
        {
            playerControls.Disable();
        }
        
        
    }

    // if we are loading into our world scene, enable our players controls
    private void OnScreneChange(Scene OldScene, Scene NewScene)
    {
        if (NewScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            Instance.enabled = true;

            if (playerControls != null)
            {
                playerControls.Enable();
            }
        }
        // otherwise we must be at the main menu, disable our player controls
        // this is so our player cant move around if we enter this like a charater creation meniu
        else
        {
            Instance.enabled = false;

            if (playerControls != null)
            {
                playerControls.Disable();
            }
        }
    }

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement1.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.RB.performed += i => RB_Input = true;

            // lock on
            playerControls.PlayerActions.LockOn.performed += i => lockOn_Input = true;

            // holding the input sets the bool to true 
            playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            // relese the inmput set the bool to false
            playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        // if we destroy this object, unsubscribe from this event
        SceneManager.activeSceneChanged -= OnScreneChange;
    }

    // if we minimize we cant control
    private void OnApplicationFocus(bool focus)
    {
        if(enabled)
        {
            if(focus)
            {
                playerControls.Enable();
            }
            else
            {
                playerControls.Disable();
            }
        }
    }

    private void Update()
    {
        HandleAllInputs();
    }

    private void HandleAllInputs()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleDodgeInput();
        HandleSprintInput();
        HandleJumpInput();
        HandleRBInput();
        HandleLockOnInput();

    }

    // lock on
    private void HandleLockOnInput()
    {
        // check for dead target 
        if(player.playerNetworkManager.isLockedOn.Value)
        {
            if(player.playerCombatManager.currentTarget == null)
            {
                return;
            }
            if(player.playerCombatManager.currentTarget.isDead.Value)
            {
                player.playerNetworkManager.isLockedOn.Value = false;
            }

            // attempt to find new target
        }

        if(lockOn_Input && player.playerNetworkManager.isLockedOn.Value)
        {
            lockOn_Input = false;
            PlayerCamera.instance.ClearLockOnTargets();
            player.playerNetworkManager.isLockedOn.Value = false;
            // disable lock on
            return;
            

        }

        if (lockOn_Input && !player.playerNetworkManager.isLockedOn.Value)
        {
            lockOn_Input = false;
            // if we are aiming using ranged weapon return (do not allow lock whilst aiming)

            // enable lock on

            PlayerCamera.instance.HandleLocatingLockOnTargets();

            if(PlayerCamera.instance.nearestLockOnTarget != null)
            {
                //set the target as our current target
                player.playerCombatManager.SetTarget(PlayerCamera.instance.nearestLockOnTarget);
                player.playerNetworkManager.isLockedOn.Value = true;
            }

        }
    }

    // movement
    private void HandlePlayerMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        // return the abslute number, number vithout minus - one plus + 
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        // we clamp the values, so thet are 0, 0.5 or 1
        if(moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;

        }
        else if(moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1;
        }
        // why do we pass 0 on the horizontal? becouse we only want non strafing movement 
        // we use the horizontal when we are strafing on loced on 
        if(player == null)
        {
            return;
        }
        // if we are not locked on, only use the move amount 
        player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);

        // if we are locked on pass the horizontal movement as well 
    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }

    // actions
    private void HandleDodgeInput()
    {
        if (dodgeInput)
        {
            dodgeInput = false;
            // future note return if meniu or ui is open (do nothing) 
            // preforme dodge
            player.playerLocomotionManager.AttempToPreformeDodge();

        }
    }

    private void HandleSprintInput()
    {
        if(sprintInput)
        {
            // handel sprinting 
            player.playerLocomotionManager.HandleSprinting();

        }
        else
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }
    }

    private void HandleJumpInput()
    {
        if(jumpInput)
        {
            jumpInput = false;

            // if we have a ui window open, simply return without doing anything

            // attemp to perfome jump
            player.playerLocomotionManager.AttempToPreformeJump();
        }
    }

    private void HandleRBInput()
    {
        if(RB_Input)
        {
            RB_Input = false;

            // TODO: IF WE HAVE A UI WINDOW OPEN RETURN AND DO NOTHING 

            player.playerNetworkManager.SetCharacterActionHand(true);

            // todo: if we are two handing the weapon use the two handed action

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RB_Action, player.playerInventoryManager.currentRightHandWeapon);
        }
    }
}
