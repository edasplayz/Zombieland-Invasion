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
    [SerializeField] float rotationSpeed = 15;

    [Header("Dodge")]
    private Vector3 rollDirection;
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
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);

            // if locked on, pass horizontal and vetical
        }
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
        GetMovementValues();
        // our move directionb is based on our cameras facing direction perspective & our movement input
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if(PlayerInputManager.Instance.moveAmount > 0.5f)
        {
            // move at a running speed
            player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
        }
        else if(PlayerInputManager.Instance.moveAmount <= 0.5f)
        {
            // move at a walking speed
            player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
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

    public void AttempToPreformeDodge()
    {
        if(player.isPreformingAction)
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
        }
    }
}
