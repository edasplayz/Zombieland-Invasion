using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIKnightCombatManager : AICharacterCombatManager
{
    [Header("Damage Colliders")]
    [SerializeField] KnightSwordDamageCollider knightSwordDamageCollider;
    [SerializeField] Transform knightsStompingFoot;
    [SerializeField] float stompAttackAOERadius = 1.5f;
   // [SerializeField] UndeadHandDamageCollider leftHandDamageCollider;


    [Header("Damage")]
    [SerializeField] int baseDamage = 25;
    [SerializeField] float attack01DamageModifier = 1.0f;
    [SerializeField] float attack02DamageModifier = 1.4f;
    [SerializeField] float attack03DamageModifier = 1.6f;
    [SerializeField] float attack04DamageModifier = 1.8f;
    [SerializeField] float stompDamage = 25;

    public void SetAttack01Damage()
    {
        knightSwordDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
      //  leftHandDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
    }

    public void SetAttack02Damage()
    {
        knightSwordDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
       // leftHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
    }

    public void SetAttack03Damage()
    {
        knightSwordDamageCollider.physicalDamage = baseDamage * attack03DamageModifier;
        // leftHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
    }

    public void SetAttack04Damage()
    {
        knightSwordDamageCollider.physicalDamage = baseDamage * attack04DamageModifier;
        // leftHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
    }

    public void OpenClubDamageCollider()
    {
        aiCharacter.characterSoundFXManager.PlayAttackGrunt();
        knightSwordDamageCollider.EnableDamageCollider();
    }

    public void CloseClubDamageCollider()
    {
        knightSwordDamageCollider.DisableDamageCollider();
    }

    public void ActivateKnightStomp()
    {
        Collider[] colliders = Physics.OverlapSphere(knightsStompingFoot.position, stompAttackAOERadius, WorldUtilityManager.Instance.GetCharacterLayers());
        List<CharacterManager> charactersDamaged = new List<CharacterManager>();


        foreach (var collider in colliders)
        {
            CharacterManager character = collider.GetComponentInParent<CharacterManager>();

            if(character != null)
            {
                if (charactersDamaged.Contains(character))
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
                    damageEffect.physicalDamage = stompDamage;
                    damageEffect.poiseDamage = stompDamage;


                    character.characterEffectsManager.ProccessInstantEffect(damageEffect);
                }
            }

            
        }
    }

    public override void PivotTowardsTarget(AICharacterManager aICharacter)
    {
        // play a pivot animation depending on viewable angle of target
        if (aICharacter.isPreformingAction)
        {
            return;
        }

        
         if (viewableAngle >= 61 && viewableAngle <= 110)
        {
            aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_R_90", true);
        }
        else if (viewableAngle <= -61 && viewableAngle >= -110)
        {
            aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_L_90", true);
        }
      
        
        else if (viewableAngle >= 146 && viewableAngle <= 180)
        {
            aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_R_180", true);
        }
        else if (viewableAngle <= -146 && viewableAngle >= -180)
        {
            aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_L_180", true);
        }
    }
}
