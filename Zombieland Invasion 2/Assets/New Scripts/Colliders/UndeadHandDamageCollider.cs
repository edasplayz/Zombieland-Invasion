using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadHandDamageCollider : DamageCollider
{

    [SerializeField] AICharacterManager undeadCharacter;

    protected override void Awake()
    {
        base.Awake();

        damageCollider = GetComponent<Collider>();
        undeadCharacter = GetComponentInParent<AICharacterManager>();
    }

    protected override void GetBlockingDotValues(CharacterManager damageTarget)
    {
        directionFromAttackToDamageTarget = undeadCharacter.transform.position - damageTarget.transform.position;
        dotValueFromAttackToDamageTarget = Vector3.Dot(directionFromAttackToDamageTarget, damageTarget.transform.position);
    }
    protected override void DamageTarget(CharacterManager damageTarget)
    {
        // we don't want to damage the same target more then once in a single attack
        // so we add them to the list that check before applieng damage

        if (charactersDamaged.Contains(damageTarget))
        {
            return;
        }

        charactersDamaged.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicDamage = magicDamage;
        damageEffect.fireDamage = fireDamage;
        damageEffect.holyDamage = holyDamage;
        damageEffect.lightningDamage = lightningDamage;
        damageEffect.poiseDamage = poiseDamage;
        damageEffect.contactPoint = contactPoint;
        damageEffect.angleHitFrom = Vector3.SignedAngle(undeadCharacter.transform.forward, damageTarget.transform.forward, Vector3.up);

       

        //damageTarget.characterEffectsManager.ProccessInstantEffect(damageEffect);

        //option 1
        // this will apply damage if a.i hits its target on the host side regardless of how it looks on any other client side
        /*if (undeadCharacter.IsOwner)
        {
            //send a damage request to the server
            damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(damageTarget.NetworkObjectId,
                undeadCharacter.NetworkObjectId,
                damageEffect.physicalDamage,
                damageEffect.magicDamage,
                damageEffect.fireDamage,
                damageEffect.holyDamage,
                damageEffect.lightningDamage,
                damageEffect.poiseDamage,
                damageEffect.angleHitFrom,
                damageEffect.contactPoint.x,
                damageEffect.contactPoint.y,
                damageEffect.contactPoint.z);
        }*/

        //option 2
        // this will apply damage if a.i hits its target on the connected characters side regardless of how it looks on any other clients side
        if (damageTarget.IsOwner)
        {
            //send a damage request to the server
            damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(damageTarget.NetworkObjectId,
                undeadCharacter.NetworkObjectId,
                damageEffect.physicalDamage,
                damageEffect.magicDamage,
                damageEffect.fireDamage,
                damageEffect.holyDamage,
                damageEffect.lightningDamage,
                damageEffect.poiseDamage,
                damageEffect.angleHitFrom,
                damageEffect.contactPoint.x,
                damageEffect.contactPoint.y,
                damageEffect.contactPoint.z);
        }
    }
}
