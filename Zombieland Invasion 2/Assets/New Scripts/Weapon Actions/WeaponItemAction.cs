using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/ Test Actions")]
public class WeaponItemAction : ScriptableObject
{


    public int actionID;

    public virtual void AttemptToPreformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        
        //  we shoud always keep track of which weapon is currently being used
        if(playerPerformingAction.IsOwner)
        {
            playerPerformingAction.playerNetworkManager.currentWeaponBeingUsed.Value = weaponPerformingAction.itemID;
        }

       // Debug.Log("The action has fired");
    }
}
