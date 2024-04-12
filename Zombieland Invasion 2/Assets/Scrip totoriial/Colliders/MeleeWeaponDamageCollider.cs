using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponDamageCollider : DamageCollider
{
    [Header("Attacking Character")]
    public CharacterManager characterCausingDamage; // when calculating damage this is used to check for attackers damage modifiers, effects ect

    [Header("Weapon Attack Modifiers")]
    public float light_Attack_01_Modifier;
    public float heavy_Attack_01_Modifier;
    public float charge_Attack_01_Modifier;
    protected override void Awake()
    {
        base.Awake();

        if(damageCollider == null)
        {
            damageCollider = GetComponent<Collider>();
        }
        damageCollider.enabled = false; // melee weapon colliders shoul be disabled at start only enable when animations allow
    }

    protected override void OnTriggerEnter(Collider other)
    {
        CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

        

        if (damageTarget != null)
        {
            //we do not want to damage ourselves
            if (damageTarget == characterCausingDamage)
            {
                return;
            }
            contactPoint = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            // check if we can damage this target based on friendly fire

            // check if target is blocking

            // check if target is invulnerable

            // damage
            DamageTarget(damageTarget);
        }
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
        damageEffect.contactPoint = contactPoint;
        damageEffect.angleHitFrom = Vector3.SignedAngle(characterCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up);

        switch (characterCausingDamage.characterCombatManager.currentAttackType)
        {
            case AttackType.LightAttack01:
                ApplyAttackDamageModifiers(light_Attack_01_Modifier, damageEffect);
                break;
            case AttackType.HeavyAttack01:
                ApplyAttackDamageModifiers(heavy_Attack_01_Modifier, damageEffect);
                break;
            case AttackType.ChargeAttack01:
                ApplyAttackDamageModifiers(charge_Attack_01_Modifier, damageEffect);
                break;
            default:
                break;
        }


        
        //damageTarget.characterEffectsManager.ProccessInstantEffect(damageEffect);

        if (characterCausingDamage.IsOwner)
        {
            //send a damage request to the server
            damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(damageTarget.NetworkObjectId,
                characterCausingDamage.NetworkObjectId,
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

    private void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage)
    {
        damage.physicalDamage *= modifier;
        damage.magicDamage *= modifier;
        damage.fireDamage *= modifier;
        damage.holyDamage *= modifier;
        damage.lightningDamage *= modifier;
        damage.poiseDamage *= modifier;

        // if attack id fully charged heavy, multiply by full charge modifier after normal modifier have been calculated
    }
}
