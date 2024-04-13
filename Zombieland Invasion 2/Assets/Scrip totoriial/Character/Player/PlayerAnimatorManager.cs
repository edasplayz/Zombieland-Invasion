using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    PlayerManager player;
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }
    private void OnAnimatorMove()
    {
        if (player.applyRootMotion)
        {
            Vector3 velocity = player.animator.deltaPosition;
            player.characterController.Move(velocity);
            player.transform.rotation *= player.animator.deltaRotation;
        }
    }

    // animation event calls
    public override void EnableCanDoCombos()
    {
        if (player.playerNetworkManager.isUsingRightHand.Value)
        {
            player.playerCombatManager.canComboWithMainHandWeapon = true;
        }
        else
        {
            // enable off hand combos
            //player.playerCombatManager.canComboWithOffHandWeapon = true;
        }
    }

    public override void DisableCnDoCombos()
    {
        player.playerCombatManager.canComboWithMainHandWeapon = false;
        //player.playerCombatManager.canComboWithOffHandWeapon = false;
    }
}
