using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightStompCollider : DamageCollider
{
    [SerializeField] AIKnightCharacterManager knightCharacterManager;

    protected override void Awake()
    {
        base.Awake();

        knightCharacterManager = GetComponentInParent<AIKnightCharacterManager>();
    }
    public void StompAttack()
    {

        GameObject stompVFX = Instantiate(knightCharacterManager.knightCombatManager.knightImpactVFX, transform);
        Collider[] colliders = Physics.OverlapSphere(transform.position, knightCharacterManager.knightCombatManager.stompAttackAOERadius, WorldUtilityManager.Instance.GetCharacterLayers());
        List<CharacterManager> charactersDamaged = new List<CharacterManager>();


        foreach (var collider in colliders)
        {
            CharacterManager character = collider.GetComponentInParent<CharacterManager>();

            if (character != null)
            {
                if (charactersDamaged.Contains(character))
                {
                    continue;
                }

                // we dont want knight to hurt himself when he stomps 
                if(character ==  knightCharacterManager)
                {
                    continue;
                }

                charactersDamaged.Add(character);

                // we only process damage if the character isowner so thay only get damaged if the collider connects on theyr client
                // meaning if you are hit on the host screen but not on your own you will not be hit
                if (character.IsOwner)
                {
                    // check for block
                    TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
                    damageEffect.physicalDamage = knightCharacterManager.knightCombatManager.stompDamage;
                    damageEffect.poiseDamage = knightCharacterManager.knightCombatManager.stompDamage;


                    character.characterEffectsManager.ProccessInstantEffect(damageEffect);
                }
            }


        }
    }
}
