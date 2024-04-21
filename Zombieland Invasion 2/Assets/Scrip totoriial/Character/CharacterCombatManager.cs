using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterCombatManager : NetworkBehaviour
{
    protected CharacterManager character;

    [Header("Last attack Animation Performed")]
    public string lastAttackAnimationPerformed;

    [Header("Attack Type")]
    public CharacterManager currentTarget;

    [Header("Attack Type")]
    public AttackType currentAttackType;

    [Header("Lock On Transform")]
    public Transform lockOnTransform;

    [Header("Attack Flags")]
    public bool canPerformRollingAttack = false;
    public bool canPerformBackStepAttack = false;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public virtual void SetTarget(CharacterManager newTarget)
    {
        if (character.IsOwner)
        {
            if(newTarget != null)
            {
                currentTarget = newTarget;
                // tell the network we have a target and tell the network who it is 
                character.characterNetworkManager.currentTargetNetworkObjectID.Value = newTarget.GetComponent<NetworkObject>().NetworkObjectId;
            }
            else
            {
                currentTarget = null;
            }
        }
        
    }

    public void EnableIsInvulnerable()
    {
        if(character.IsOwner)
        {
            character.characterNetworkManager.isInvulnerable.Value = true;
        }
        
    }

    public void DisableIsInvulnerable() 
    { 
        if(character.IsOwner)
        {
            character.characterNetworkManager.isInvulnerable.Value = false;
        }
        
    }

    public void EnableCanDoRollinAttack()
    {
        // if (character.IsOwner)
        // {
        canPerformRollingAttack = true;
        // }

    }

    public void DisableCanDoRollinAttack()
    {
        // if (character.IsOwner)
        // {
        canPerformRollingAttack = false;
        // }

    }

    public void EnableCanDoBackStepAttack()
    {
        // if (character.IsOwner)
        // {
        canPerformBackStepAttack = true;
        // }

    }

    public void DisableCanDoBackStepAttack()
    {
        // if (character.IsOwner)
        // {
        canPerformBackStepAttack = false;
        // }

    }

    public virtual void EnableCanDoCombos()
    {

    }

    public virtual void DisableCnDoCombos()
    {

    }

}
