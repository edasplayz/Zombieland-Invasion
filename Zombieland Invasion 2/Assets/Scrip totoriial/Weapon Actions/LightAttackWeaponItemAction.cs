using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/ Light Attack Action")]
public class LightAttackWeaponItemAction : WeaponItemAction
{
    [Header("Light Attacks")]
    [SerializeField] string light_Attack_01 = "Main_Light_Attack_01"; // right hand
    [SerializeField] string light_Attack_02 = "Main_Light_Attack_02";
    [SerializeField] string light_Attack_03 = "Main_Light_Attack_03";

    [Header("Running Attacks")]
    [SerializeField] string run_Attack_01 = "Main_Run_Attack_01";

    [Header("Rolling Attacks")]
    [SerializeField] string roll_Attack_01 = "Main_Roll_Attack_01";

    [Header("Backstep Attacks")]
    [SerializeField] string backstep_Attack_01 = "Main_BacStep_Attack_01 ";
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

        if(!playerPerformingAction.characterLocomationManager.isGrounded)
        {
            return;
        }

        // if we are sprinting perform a running atack
        if (playerPerformingAction.characterNetworkManager.isSprinting.Value)
        {
            PerformRunningAttack(playerPerformingAction, weaponPerformingAction);
            return;
        }

        // if we are rolling perform a rolling atack
        if (playerPerformingAction.characterCombatManager.canPerformRollingAttack)
        {
            PerformRollingAttack(playerPerformingAction, weaponPerformingAction);
            return;
        }

        // if we are backsteping perform a backstep atack
        if (playerPerformingAction.characterCombatManager.canPerformBackStepAttack)
        {
            PerformBackstepAttack(playerPerformingAction, weaponPerformingAction);
            return;
        }

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
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack02, light_Attack_02, true);
            }
            else if(playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_02)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack03, light_Attack_03, true);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack01, light_Attack_01, true);
            }
        }
        // otherwise if we are not already attacking just perform a regular attack
        else if (!playerPerformingAction.isPreformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack01, light_Attack_01, true);
        }
        
        
    }

    private void PerformRunningAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        // if we are two handing our weapon perform a two hand run attack (to do)
        // else perform a one hand tun attack

        playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RunningAttack01, run_Attack_01, true);


    }

    private void PerformRollingAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        // if we are two handing our weapon perform a two hand run attack (to do)
        // else perform a one hand tun attack
        playerPerformingAction.playerCombatManager.canPerformRollingAttack = false;
        playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RollingAttack01, roll_Attack_01, true);


    }

    private void PerformBackstepAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        // if we are two handing our weapon perform a two hand run attack (to do)
        // else perform a one hand tun attack
        playerPerformingAction.playerCombatManager.canPerformBackStepAttack = false;
        playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.BackstepAttack01, backstep_Attack_01, true);


    }
}
