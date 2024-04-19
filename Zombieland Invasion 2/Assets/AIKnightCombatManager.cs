using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIKnightCombatManager : AICharacterCombatManager
{
    [Header("Damage Colliders")]
    [SerializeField] KnightSwordDamageCollider knightSwordDamageCollider;
   // [SerializeField] UndeadHandDamageCollider leftHandDamageCollider;

    [Header("Damage")]
    [SerializeField] int baseDamage = 25;
    [SerializeField] float attack01DamageModifier = 1.0f;
    [SerializeField] float attack02DamageModifier = 1.4f;
    [SerializeField] float attack03DamageModifier = 1.6f;
    [SerializeField] float attack04DamageModifier = 1.8f;

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

    }
}
