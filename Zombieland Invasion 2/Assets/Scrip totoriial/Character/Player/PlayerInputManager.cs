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
    [SerializeField] bool lockOn_Left_Input;
    [SerializeField] bool lockOn_Right_Input;
    private Coroutine lockOnCorotine;
    [SerializeField] float snapThreshold = 25f;

    [Header("Player action input")]
    [SerializeField] bool dodgeInput = false;
    [SerializeField] bool sprintInput = false;
    [SerializeField] bool jumpInput = false;
    [SerializeField] bool switch_Right_Weapon_Input = false;
    [SerializeField] bool switch_Left_Weapon_Input = false;
    [SerializeField] bool interaction_Input = false;

    [Header("Qued inputs")]
    [SerializeField] private bool input_Que_Is_Active = false; 
    [SerializeField] float que_Input_Timer = 0;
    [SerializeField] float default_Que_Input_Timer = 0.35f;
    [SerializeField] bool que_RB_Input = false;
    [SerializeField] bool que_RT_Input = false;

    [Header("Bumper Inputs")]
    [SerializeField] bool RB_Input = false;

    [Header("Trigger Inputs")]
    [SerializeField] bool RT_Input = false;
    [SerializeField] bool Hold_RT_Input = false;

    

    

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

            // actions
            playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.SwitchRightWeapon.performed += i => switch_Right_Weapon_Input = true;
            playerControls.PlayerActions.SwitchLeftWeapon.performed += i => switch_Left_Weapon_Input = true;
            playerControls.PlayerActions.Interact.performed += i => interaction_Input = true;

            // triggers
            playerControls.PlayerActions.HoldRT.performed += i => Hold_RT_Input = true;
            playerControls.PlayerActions.HoldRT.canceled += i => Hold_RT_Input = false;

            // bumpers
            playerControls.PlayerActions.RB.performed += i => RB_Input = true;

            // lock on
            playerControls.PlayerActions.RT.performed += i => RT_Input = true;
            playerControls.PlayerActions.LockOn.performed += i => lockOn_Input = true;
            playerControls.PlayerActions.SeekLeftLockedOnTarget.performed += i => lockOn_Left_Input = true;
            playerControls.PlayerActions.SeekRightLockedOnTarget1.performed += i => lockOn_Right_Input = true;

            // should be mouse controls when locked on 
            playerControls.PlayerCamera.Movement1.performed += i =>
            {
                Vector2 mouseDelta = i.ReadValue<Vector2>();

                // Check if the mouse movement exceeds the snap threshold
                if (Mathf.Abs(mouseDelta.x) > snapThreshold)
                {
                    // Check if the mouse movement is to the left and exceeds the snap threshold
                    if (mouseDelta.x < 0)
                    {
                        lockOn_Left_Input = true;
                        lockOn_Right_Input = false; // Reset right input if moving left
                    }
                    // Check if the mouse movement is to the right and exceeds the snap threshold
                    else if (mouseDelta.x > 0)
                    {
                        lockOn_Right_Input = true;
                        lockOn_Left_Input = false; // Reset left input if moving right
                    }
                }
                else
                {
                    // If no significant horizontal movement, reset both inputs
                    lockOn_Left_Input = false;
                    lockOn_Right_Input = false;
                }

                // Assign mouse delta movement input to cameraInput
                cameraInput = mouseDelta;
            };




            // holding the input sets the bool to true 
            playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            // relese the inmput set the bool to false
            playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;

            // qued inputs
            playerControls.PlayerActions.QueRB.performed += i => QueInput(ref que_RB_Input);
            playerControls.PlayerActions.QueRT.performed += i => QueInput(ref que_RT_Input);

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
        HandleLockOnSwitchTargetInput();
        HandleRTInput();
        HandleChargeRTInput();
        HandleSwitchRightWeaponInput();
        HandleSwitchLeftWeaponInput();
        HandleQuedInputs();
        HandleInteractionInput();
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
                // attempt to find new target

                // this assures us that the coroutine nevert runs multiple times overlaping itself
                if (lockOnCorotine != null)
                {
                    StopCoroutine(lockOnCorotine);
                }
                lockOnCorotine = StartCoroutine(PlayerCamera.instance.WaitThenFindNewTarget());

            }

            
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

    private void HandleLockOnSwitchTargetInput()
    {
        if (lockOn_Left_Input)
        {
            lockOn_Left_Input = false;

            if (player.playerNetworkManager.isLockedOn.Value)
            {
                PlayerCamera.instance.HandleLocatingLockOnTargets();

                if(PlayerCamera.instance.leftLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.instance.leftLockOnTarget);
                }
            }
        }

        if (lockOn_Right_Input)
        {
            lockOn_Right_Input = false;

            if (player.playerNetworkManager.isLockedOn.Value)
            {
                PlayerCamera.instance.HandleLocatingLockOnTargets();

                if (PlayerCamera.instance.rightLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.instance.rightLockOnTarget);
                }
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

        if(moveAmount != 0)
        {
            player.playerNetworkManager.isMoving.Value = true;
        }
        else
        {
            player.playerNetworkManager.isMoving.Value = false;
        }

        // if we are not locked on, only use the move amount 
        if (!player.playerNetworkManager.isLockedOn.Value || player.playerNetworkManager.isSprinting.Value)
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
        }
        else
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput, player.playerNetworkManager.isSprinting.Value);

        }
        

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

    private void HandleRTInput()
    {
        if (RT_Input)
        {
            RT_Input = false;

            // TODO: IF WE HAVE A UI WINDOW OPEN RETURN AND DO NOTHING 

            player.playerNetworkManager.SetCharacterActionHand(true);

            // todo: if we are two handing the weapon use the two handed action

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RT_Action, player.playerInventoryManager.currentRightHandWeapon);
        }
    }

    private void HandleChargeRTInput()
    {
        // we only want to check for a charge if we are in an action that requiers it (attacking)
        if (player.isPreformingAction)
        {
            player.playerNetworkManager.isChargingAttack.Value = Hold_RT_Input;
        }
    }

    private void HandleSwitchRightWeaponInput()
    {
        if (switch_Right_Weapon_Input)
        {
            switch_Right_Weapon_Input = false;
            player.playerEquipmentManager.SwitchRightWeapon();
        }
    }

    private void HandleSwitchLeftWeaponInput()
    {
        if (switch_Left_Weapon_Input)
        {
            switch_Left_Weapon_Input = false;
            player.playerEquipmentManager.SwitchLeftWeapon();
        }
    }

    private void HandleInteractionInput()
    {
        if(interaction_Input)
        {
            interaction_Input = false;

            player.playerInteractionManager.Interact();
        }
    }

    private void QueInput(ref bool quedInput) // passing a reference means we pass a specific bool and not  the value og that bool (true or false)
    {
        // reset all qued inputs so only one can que at a time
        que_RB_Input = false;
        que_RT_Input = false;
        //que_LB_Input = false;
        //que_LT_Input = false;

        // check for ui window being open if its open return

        if(player.isPreformingAction || player.playerNetworkManager.isJumping.Value)
        {
            quedInput = true;
            // attempt this new input for x amount of time
            que_Input_Timer = default_Que_Input_Timer;
            input_Que_Is_Active = true;
        }
    }

    private void ProcessQuedInputs()
    {
        if (player.isDead.Value)
        {
            return;
        }
        if(que_RB_Input)
        {
            RB_Input = true;
        }
        if (que_RT_Input)
        {
            RB_Input = true;
        }
    }

    private void HandleQuedInputs()
    {
        if (input_Que_Is_Active)
        {
            // while the timer is above keep attempting to press the input{
            if(que_Input_Timer > 0)
            {
                que_Input_Timer -= Time.deltaTime;
                ProcessQuedInputs();
            }
            else
            {
                // reset all qued inputs
                que_RB_Input = false;
                que_RT_Input = false;

                input_Que_Is_Active = false;
                que_Input_Timer = 0;
            }
        }
    }
}
