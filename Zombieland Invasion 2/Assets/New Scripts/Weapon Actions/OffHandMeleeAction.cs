using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Off Hand Melee Action")]
public class OffHandMeleeAction : WeaponItemAction
{
    public override void AttemptToPreformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttemptToPreformAction(playerPerformingAction, weaponPerformingAction);

        // check for power stance action

        // check for can block
        if (!playerPerformingAction.playerCombatManager.canBlock)
        {
            return;
        }

        // check for attack status
        if (playerPerformingAction.playerNetworkManager.isAttacking.Value)
        {
            // disable blocking 
            if (playerPerformingAction.IsOwner)
            {
                playerPerformingAction.playerNetworkManager.isBlocking.Value = false;

                return;
            }
        }

        if (playerPerformingAction.playerNetworkManager.isBlocking.Value)
        {
            return;
        }

        if(playerPerformingAction.IsOwner)
        {
           
            playerPerformingAction.playerNetworkManager.isBlocking.Value = true;
        }
    }
}
