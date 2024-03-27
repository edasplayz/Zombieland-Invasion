using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerNetworkManager playerNetworkManager;
    protected override void Awake()
    {
        base.Awake();

        // do more stuff, only for the player

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
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
        }
    }
}
