using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Craracter Effects/Instant Effects/Take Blocked Damage")]
public class TakeBlockedDamageEffect : InstantCharacterEffect
{
    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage; // if the damage is caused by another character attack it will be stored here

    [Header("Damage")]

    public float physicalDamage = 0; // on the future will be split into standart, stike , slash and pierce
    public float magicDamage = 0;
    public float fireDamage = 0;
    public float lightningDamage = 0;
    public float holyDamage = 0;


    [Header("Final Damage")]
    private int finalDamage = 0; // the damage the character takes after all calculations have benn made

    [Header("Poise")]
    public float poiseDamage = 0;
    public bool poiseIsBroken = false; // if a character poise is broken, they will be stunned and play a damage animation
    // to do
    // build ups
    // build up effects amount

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("Sound FX")]
    public bool willPlayDamageSFX = true;
    public AudioClip elementalDamageSoundFX; // used on top of regular sfx if there is elemental damage present (magic/fire/lighting/holy)

    [Header("Direction Damage Taken From")]
    public float angleHitFrom; // used to determine what damage animation to play ( move backwards to the left to the right ect)
    public Vector3 contactPoint; // used to determine where the blood fx instantiate


    public override void ProccesEffect(CharacterManager character)
    {
        // check for invulnerability
        if (character.characterNetworkManager.isInvulnerable.Value)
        {
            return;
        }
        base.ProccesEffect(character);

        // if the character is dead, no additional damage effect should be processed
        if (character.isDead.Value)
        {
            return;
        }

        Debug.Log("Hit Was Blocked!");

        // calculate damage
        CalculateDamage(character);
        // check whick direction damage came from
        // play a damage animation
        PlayDirectionalBasedBlockingAnimation(character);
        // check for build ups (poison bleed)
        // play damage sound fx
        PlayDamageSFX(character);
        // play damage vfx (blood)
        PlayDamageVFX(character);

        // if character is a.i check for new target if character causing damage is present
    }

    private void CalculateDamage(CharacterManager character)
    {
        if (!character.IsOwner)
        {
            return;
        }

        if (characterCausingDamage != null)
        {
            // check for damage modifiers and modify base damage (physical dmage buff, elemental damage buff ect)
            // physical += physicalModifier ect


        }

        // check character for flat difenses and subbtract fhem from the damage

        // check character for armor absorbtion, and subtract the percentage from the damage 

        // add all damage types together, and apply final damage
        Debug.Log("Original physical damage" + physicalDamage);

        physicalDamage -= (physicalDamage * (character.characterStatManager.blockingPhysicalAbsobtion / 100));
        magicDamage -= (magicDamage * (character.characterStatManager.blockingMagicAbsobtion / 100));   
        fireDamage -= (fireDamage * (character.characterStatManager.blockingFireAbsobtion / 100));
        lightningDamage -= (lightningDamage * (character.characterStatManager.blockingLightningAbsobtion / 100));
        holyDamage -= (holyDamage * (character.characterStatManager.blockingHolyAbsobtion / 100));

        finalDamage = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

        if (finalDamage <= 0)
        {
            finalDamage = 1;
        }

        Debug.Log("final physical damage" + physicalDamage);

        character.characterNetworkManager.currentHealth.Value -= finalDamage;

        // calculate poise damage to determine if the character will be stunned 
       // Debug.Log("Target has taken " + finalDamage + " damage.");
    }

    private void PlayDamageVFX(CharacterManager character)
    {
        // if we have fire damage play fire particle
        // lightning damage, lightning particle ect

        // get vfx based on blocking weapon
        
    }

    private void PlayDamageSFX(CharacterManager character)
    {
        
        // if fire damage is greater than 0 play burn sfx
        // if lightning famage is greater then 0 play zap sfx

        // get sfx based on blocking weapon
    }

    private void PlayDirectionalBasedBlockingAnimation(CharacterManager character)
    {
        if (!character.IsOwner)
        {
            return;
        }

        if (character.isDead.Value)
        {
            return;
        }

        // calculate an intensity based on poise damage
        DamageIntensity damageIntensity = WorldUtilityManager.Instance.GetDamageIntensityBasedOnPoiseDamage(poiseDamage);
        // play propper animation to match the intensity of the blow 

        // todo: check for two hand status if two handing use two hands version of block anim instead

        switch (damageIntensity)
        {
            case DamageIntensity.Ping:
                damageAnimation = "Block_Ping_01";
                break;
            case DamageIntensity.Light:
                damageAnimation = "Block_Light_01";
                break;
            case DamageIntensity.Medium:
                damageAnimation = "Block_Medium_01";
                break;
            case DamageIntensity.Heavy:
                damageAnimation = "Block_Heavy_01";
                break;
            case DamageIntensity.Colossal:
                damageAnimation = "Block_Colossal_01";
                break;
            default:
                break;
        }



        // if poise is broken play a staggering damage animation
        character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;
        character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
    }
}
