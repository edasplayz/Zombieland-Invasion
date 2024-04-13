using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/ Light Attack Action")]
public class LightAttackWeaponItemAction : WeaponItemAction
{
    [SerializeField] string light_Attack_01 = "Main_Light_Attack_01"; // right hand
    [SerializeField] string light_Attack_02 = "Main_Light_Attack_02";
    [SerializeField] string light_Attack_03 = "Main_Light_Attack_03";
    public override void AttemptToPreformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {

        base.AttemptToPreformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner)
        {
            return;
        }
        // check for stops

        if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0 )
        {
            return;
        }

        /*if(!playerPerformingAction.isGrounded )
        {
            return;
        }*/

        PerformLightAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        // if we are attacking currently and we can combo perform the combo attack
        if(playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPreformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;

            // perform an attack based on the previos attack we just played
            if(playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_01 )
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack02, light_Attack_02, true);
            }
            else if(playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_02)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack03, light_Attack_03, true);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01, light_Attack_01, true);
            }
        }
        // otherwise if we are not already attacking just perform a regular attack
        else if (!playerPerformingAction.isPreformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01, light_Attack_01, true);
        }
        
        
    }
}
