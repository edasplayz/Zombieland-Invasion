using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Damage Grunts")]
    [SerializeField] protected AudioClip[] damageGrunts;

    [Header("Attack Grunts")]
    [SerializeField] protected AudioClip[] attackGrunts;

    [Header("FootSteps")]
    [SerializeField] protected AudioClip[] footSteps;
    //method 2
    //public AudioClip[] footSteps;
    //public AudioClip[] footStepsDirt;
    //public AudioClip[] footStepsStone;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundFX(AudioClip soundFX, float volume = 1, bool randomizePitch = true, float pitchRandom = 0.1f)
    {
        audioSource.PlayOneShot(soundFX, volume);
        //resets pitch
        audioSource.pitch = 1;

        if(randomizePitch)
        {
            audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
        }
    }

    public void PlayRollSoundFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSFX);
    }

    public virtual void PlayDamageGruntSoundFX()
    {
        if(damageGrunts.Length > 0) 
        {
            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(damageGrunts));
        }
        
    }

    public virtual void PlayAttackGruntSoundFX()
    {
        if(attackGrunts.Length > 0)
        {
            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(attackGrunts));
        }
        
    }

    public virtual void PlayFootStepSoundFX()
    {
        if (footSteps.Length > 0)
        {
            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(footSteps));
        }
    }

    public virtual void PlayBlockSoundFX()
    {

    }
}