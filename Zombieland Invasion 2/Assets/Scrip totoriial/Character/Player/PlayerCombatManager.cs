using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager player;
    public WeaponItem currentWeaponBeingUsed;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }
    public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
    {
        // perform the action
        weaponAction.AttemptToPreformAction(player, weaponPerformingAction);

        // notify the server we have performed the action perform the action on  other connected clients
    }
}
