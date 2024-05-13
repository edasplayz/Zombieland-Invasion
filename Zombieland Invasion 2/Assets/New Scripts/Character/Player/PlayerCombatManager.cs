using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager player;
    public WeaponItem currentWeaponBeingUsed;

    [Header("Flags")]
    public bool canComboWithMainHandWeapon = false;
    //public bool canComboWithOffHandWeapon = false;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }
    public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
    {

        if (player.IsOwner)
        {
            // perform the action
            weaponAction.AttemptToPreformAction(player, weaponPerformingAction);

            // notify the server we have performed the action perform the action on  other connected clients
            player.playerNetworkManager.NotifyTheServerOfWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId, weaponAction.actionID, weaponPerformingAction.itemID);
        }
        
    }

    public virtual void DrainStaminaBasedOnAttack()
    {
        if (!player.IsOwner)
        {
            return;
        }

        if(currentWeaponBeingUsed == null)
        {
            return;
        }

        float staminaDeducted = 0;

        switch (currentAttackType)
        {
            case AttackType.LightAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                break;
            case AttackType.LightAttack02:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                break;
            case AttackType.LightAttack03:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                break;
            case AttackType.HeavyAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                break;
            case AttackType.HeavyAttack02:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                break;
            case AttackType.ChargeAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargeAttackStaminaCostMultiplier;
                break;
            case AttackType.ChargeAttack02:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargeAttackStaminaCostMultiplier;
                break;
            case AttackType.RunningAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.runningAttackStaminaCostMultiplier;
                break;
            case AttackType.RollingAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.rollingAttackStaminaCostMultiplier;
                break;
            case AttackType.BackstepAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.backstepAttackStaminaCostMultiplier;
                break;
            default:
                break;
        }



        Debug.Log("Stamina deducted: " + staminaDeducted);
        player.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaDeducted);
        
    }

    public override void SetTarget(CharacterManager newTarget)
    {
        base.SetTarget(newTarget);

        if (player.IsOwner)
        {
            PlayerCamera.instance.SetLockCameraHeight();
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
