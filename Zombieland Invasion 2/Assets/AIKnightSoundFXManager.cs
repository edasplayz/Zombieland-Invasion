using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIKnightSoundFXManager : CharacterSoundFXManager
{
    [Header("Sword Whooshes")]
    public AudioClip[] swordWhooshes;

    [Header("Sword Impact")]
    public AudioClip[] swordImpact;

    [Header("Stomp Impact")]
    public AudioClip[] stompImpact;

    public virtual void PlaySwordImpactSoundFX()
    {
        if (swordImpact.Length > 0)
        {
            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(swordImpact));
        }
    }

    public virtual void PlayStompImpactSoundFX()
    {
        if (stompImpact.Length > 0)
        {
            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(stompImpact));
        }
    }
}
