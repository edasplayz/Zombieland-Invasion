using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Craracter Effects/Instant Effects/Take Damage")]
public class TakeDamageEffect : InstantCharacterEffect
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
        base.ProccesEffect(character);

        // if the character is dead, no additional damage effect should be processed
        if(character.isDead.Value)
        {
            return;
        }

        // check for invulnerability

        // calculate damage
        CalculateDamage(character);
        // check whick direction damage came from
        // play a damage animation
        PlayDirectionalBasedDamageAnimation(character);
        // check for build ups (poison bleed)
        // play damage sound fx
        PlayDamageSFX(character);
        // play damage vfx (blood)
        PlayDamageVFX(character);

        // if character is a.i check for new target if character causing damage is present
    }

    private void CalculateDamage(CharacterManager character)
    {
        if(!character.IsOwner) 
        { 
            return; 
        }

        if(characterCausingDamage != null)
        {
            // check for damage modifiers and modify base damage (physical dmage buff, elemental damage buff ect)
            // physical += physicalModifier ect


        }

        // check character for flat difenses and subbtract fhem from the damage

        // check character for armor absorbtion, and subtract the percentage from the damage 

        // add all damage types together, and apply final damage
        finalDamage = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

        if(finalDamage <= 0)
        {
            finalDamage = 1;
        }

        

        character.characterNetworkManager.currentHealth.Value -= finalDamage;

        // calculate poise damage to determine if the character will be stunned 
    }

    private void PlayDamageVFX(CharacterManager character)
    {
        // if we have fire damage play fire particle
        // lightning damage, lightning particle ect

        character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
    }

    private void PlayDamageSFX(CharacterManager character)
    {
        AudioClip physicalDamageSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.physicalDamageSFX);
        character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX);
        // if fire damage is greater than 0 play burn sfx
        // if lightning famage is greater then 0 play zap sfx
    }

    private void PlayDirectionalBasedDamageAnimation(CharacterManager character)
    {
        if(!character.IsOwner) 
        { 
            return; 
        }

        if(character.isDead.Value)
        {
            return;
        }

        // calculate if poise is broken
        poiseIsBroken = true;
        if(angleHitFrom >= 145 && angleHitFrom <= 180)
        {
            // play front animation
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
        }
        else if (angleHitFrom <= -145 && angleHitFrom >= -180)
        {
            // play front animation
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
        }
        else if (angleHitFrom >= -45 && angleHitFrom <= 45)
        {
            // play back animation
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.backward_Medium_Damage);
        }
        else if (angleHitFrom >= -144 && angleHitFrom <= -45)
        {
            // play left animation
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.left_Medium_Damage);
        }
        else if (angleHitFrom >= 45 && angleHitFrom <= 144)
        {
            // play right animation
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.right_Medium_Damage);
        }

        // if poise is broken play a staggering damage animation
        if(poiseIsBroken)
        {
            character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;
            character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
        }
    }
}
