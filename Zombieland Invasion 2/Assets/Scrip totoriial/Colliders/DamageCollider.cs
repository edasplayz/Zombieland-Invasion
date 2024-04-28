using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{

    [Header("Collider")]
    [SerializeField] protected Collider damageCollider;

    [Header("Damage")]
    public float physicalDamage = 0; // on the future will be split into standart, stike , slash and pierce
    public float magicDamage = 0;
    public float fireDamage = 0;
    public float lightningDamage = 0;
    public float holyDamage = 0;

    [Header("Contack Point")]
    protected Vector3 contactPoint;

    [Header("Character Damaged")]
    protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

    [Header("Block")]
    protected Vector3 directionFromAttackToDamageTarget;
    protected float dotValueFromAttackToDamageTarget;

    protected virtual void Awake()
    {

    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

        // if you want to search on both the damageble character colliders & the character controller collider just for null here and do the following
        /*
        if(damageTarget == null)
        {
            damageTarget = other.GetComponent<CharacterManager>();
        }
        */

        if(damageTarget != null)
        {
            contactPoint = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            // check if we can damage this target based on friendly fire

            // check if target is blocking
            CheckForBlock(damageTarget);

            // check if target is invulnerable
            

            // damage
            DamageTarget(damageTarget);
        }
    }

    protected virtual void CheckForBlock(CharacterManager damageTarget)
    {
        // if this character has already been damaged do not proceed
        if(charactersDamaged.Contains(damageTarget))
        {
            return;
        }

        GetBlockingDotValues(damageTarget);

        // 1 check if the character being damaged is blocked
        if(damageTarget.characterNetworkManager.isBlocking.Value && dotValueFromAttackToDamageTarget > 0.3f)
        {
            // if the character is blocking check if they are facing in the correct direction to block successfully

            charactersDamaged.Add(damageTarget);
            TakeBlockedDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeBlockedDamageEffect);

            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.contactPoint = contactPoint;

            // apply blocked character damage to target
            damageTarget.characterEffectsManager.ProccessInstantEffect(damageEffect);
        }

        
    }

    protected virtual void GetBlockingDotValues(CharacterManager damageTarget)
    {
        directionFromAttackToDamageTarget = transform.position - damageTarget.transform.position;
        dotValueFromAttackToDamageTarget = Vector3.Dot(directionFromAttackToDamageTarget, damageTarget.transform.position);
    }


    protected virtual void DamageTarget(CharacterManager damageTarget)
    {
        // we don't want to damage the same target more then once in a single attack
        // so we add them to the list that check before applieng damage

        if(charactersDamaged.Contains(damageTarget))
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

        damageTarget.characterEffectsManager.ProccessInstantEffect(damageEffect);

    }

    public virtual void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public virtual void DisableDamageCollider()
    {
        damageCollider.enabled = false;
        charactersDamaged.Clear(); // we reset the character that have been hit when we reset the collider so they may be hit again
    }
}
