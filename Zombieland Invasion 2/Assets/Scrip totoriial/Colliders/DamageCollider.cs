using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
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

    private void OnTriggerEnter(Collider other)
    {
        CharacterManager damageTarget = other.GetComponent<CharacterManager>();

        if(damageTarget != null)
        {
            contactPoint = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            // check if we can damage this target based on friendly fire

            // check if target is blocking

            // check if target is invulnerable

            // damage
            DamageTarget(damageTarget);
        }
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
}
