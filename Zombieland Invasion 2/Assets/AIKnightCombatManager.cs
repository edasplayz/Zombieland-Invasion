using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIKnightCombatManager : AICharacterCombatManager
{
    AIKnightCharacterManager aiKnightManager;

    [Header("Damage Colliders")]
    [SerializeField] KnightSwordDamageCollider knightSwordDamageCollider;
    [SerializeField] KnightStompCollider knightStompCollider;
    public float stompAttackAOERadius = 1.5f;
   // [SerializeField] UndeadHandDamageCollider leftHandDamageCollider;


    [Header("Damage")]
    [SerializeField] int baseDamage = 25;
    [SerializeField] float attack01DamageModifier = 1.0f;
    [SerializeField] float attack02DamageModifier = 1.4f;
    [SerializeField] float attack03DamageModifier = 1.6f;
    [SerializeField] float attack04DamageModifier = 1.8f;
    public float stompDamage = 25;

    [Header("VFX")]
    public GameObject knightImpactVFX;
    [SerializeField] GameObject vfxSpawnPoint;

    protected override void Awake()
    {
        base.Awake();

        aiKnightManager = GetComponent<AIKnightCharacterManager>();
    }

    public void SetAttack01Damage()
    {
        aiCharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
        knightSwordDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
      //  leftHandDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
    }

    public void SetAttack02Damage()
    {
        aiCharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
        knightSwordDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
       // leftHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
    }

    public void SetAttack03Damage()
    {
        aiCharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
        knightSwordDamageCollider.physicalDamage = baseDamage * attack03DamageModifier;
        // leftHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
    }

    public void SetAttack04Damage()
    {
        aiCharacter.characterSoundFXManager.PlayAttackGruntSoundFX();

        
        knightSwordDamageCollider.physicalDamage = baseDamage * attack04DamageModifier;
        // leftHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
    }

    public void OpenClubDamageCollider()
    {
        
        knightSwordDamageCollider.EnableDamageCollider();
        aiKnightManager.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(aiKnightManager.knightSoundFXManager.swordWhooshes));
    }

    public void CloseClubDamageCollider()
    {
        knightSwordDamageCollider.DisableDamageCollider();
    }

    public void ActivateKnightStomp()
    {
        GameObject stompVFX = Instantiate(knightImpactVFX, vfxSpawnPoint.transform);


        knightStompCollider.StompAttack();
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
