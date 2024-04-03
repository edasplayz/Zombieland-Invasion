using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomationManager
{
    PlayerManager player;

    [HideInInspector] public float verticalMovement;
    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float moveAmount;

    [Header("Movement Settings")]
    private Vector3 moveDirection;
    private Vector3 targerRotationDirection;
    [SerializeField] float walkingSpeed = 2;
    [SerializeField] float runningSpeed = 5;
    [SerializeField] float sprintingSpeed = 6.5f;
    [SerializeField] float rotationSpeed = 15;
    [SerializeField] int sprintingStaminaCost = 2;


    [Header("Dodge")]
    private Vector3 rollDirection;
    [SerializeField] float dodgeStaminaCost = 25;
    [SerializeField] float jumpStaminaCost = 25;
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    protected override void Update()
    {
        base.Update();

        if(player.IsOwner)
        {
            player.characterNetworkManager.verticalMovement.Value = verticalMovement;
            player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
            player.characterNetworkManager.moveAmount.Value = moveAmount;
        }
        else
        {
            
            verticalMovement = player.characterNetworkManager.verticalMovement.Value;
            horizontalMovement = player.characterNetworkManager.verticalMovement.Value;
            moveAmount = player.characterNetworkManager.moveAmount.Value;

            // if not locked on,, pass move amount
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);

            // if locked on, pass horizontal and vetical
        }
        /*if(moveAmount <= 0)
        {
            // if we are stationary set sprinting to false
            player.playerNetworkManager.isSprinting.Value = false;
            Debug.Log("This is a debug message.");
        }*/
        
    }
    public void HandleAllMovement()
    {
        // grounded movement
        
        HandleGroundedMovement();
        HandleRotation();
        // ariel movement

    } 

    private void GetMovementValues()
    {
        verticalMovement = PlayerInputManager.Instance.verticalInput;
        horizontalMovement = PlayerInputManager.Instance.horizontalInput;
        moveAmount = PlayerInputManager.Instance.moveAmount;
        // clamp the movement
    }

    private void HandleGroundedMovement()
    {
        if (!player.canMove)
        {
            return;
        }
        GetMovementValues();
        
        // our move directionb is based on our cameras facing direction perspective & our movement input
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if(player.playerNetworkManager.isSprinting.Value)
        {
            player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
        }
        else
        {
            if (PlayerInputManager.Instance.moveAmount > 0.5f)
            {
                // move at a running speed
                player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }
            else if (PlayerInputManager.Instance.moveAmount <= 0.5f)
            {
                // move at a walking speed
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
        }

        
    }

    private void HandleRotation()
    {
        if(!player.canRotate)
        {
            return;
        }
        targerRotationDirection = Vector3.zero;
        targerRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
        targerRotationDirection = targerRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
        targerRotationDirection.Normalize();
        targerRotationDirection.y = 0;

        if(targerRotationDirection == Vector3.zero)
        {
            targerRotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(targerRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }

    public void HandleSprinting()
    {
        if (player.isPreformingAction)
        {
            // set sprinting to false if performing an action
            player.playerNetworkManager.isSprinting.Value = false;
        }

        // if we are out of stamina set sprinting to false 
        if(player.playerNetworkManager.currentStamina.Value <= 0)
        {
            player.playerNetworkManager.isSprinting.Value = false;
            return;
        }

        if (moveAmount >= 0.5)
        {
            // if we are moving set sprinting is true
            player.playerNetworkManager.isSprinting.Value = true;
            
        }
        else
        {
            // if we are stationary set sprinting to false
            player.playerNetworkManager.isSprinting.Value = false;
           // Debug.Log("This is a debug message.");
        }

        if(player.playerNetworkManager.isSprinting.Value)
        {
            player.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
        }

    }

    public void AttempToPreformeDodge()
    {
        if(player.isPreformingAction)
        {
            return;
        }

        if(player.playerNetworkManager.currentStamina.Value <= 0)
        {
            return;
        }
        // if we are moving wehn we attempt to dodge we preforme a roll
        if (PlayerInputManager.Instance.moveAmount > 0)
        {

            rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.Instance.verticalInput;
            rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.Instance.horizontalInput;

            rollDirection.y = 0;
            rollDirection.Normalize();
            Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
            player.transform.rotation = playerRotation;

            // preforme a roll animation
            player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true, true);
        }
        // if we are stationary we preform backstep
        else
        {
            // preforme a backstep animation
            player.playerAnimatorManager.PlayTargetActionAnimation("Back_Step_01", true, true);
        }

        player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;
    }

    public void AttempToPreformeJump()
    {
        // if we are performing general action we do not want to allow jump (will change when cambat is added)
        if (player.isPreformingAction)
        {
            return;
        }

        //  id we are out of stamina, we do not wish to allow a jump
        if (player.playerNetworkManager.currentStamina.Value <= 0)
        {
            return;
        }
        // if we are already jumping no more jump we need to waint for the jump to finish
        if (player.isJumping)
        {
            return;
        }
        // if we are not grounded we do not want to allow jump
        if (player.isGrounded)
        {
            return;
        }

        // if we are two handling our weapon, play two handed jump animation, otherwise play one handed animation (to do)
        player.playerAnimatorManager.PlayTargetActionAnimation("Main_jump_01", false);

        player.isJumping = true;

        player.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;

    }

    public void ApplyJumpingVelocity()
    {
        // applly an upward velocity depending on forces in our game

    }
}
