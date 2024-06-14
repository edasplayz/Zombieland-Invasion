using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    // process instant effects (take damage, heal)

    // proccess timed effect (poison, build ups) to do

    // process tatic effect (armor, removing buffs from talisman ect) to do

    CharacterManager character;

    [Header("VFX")]
    [SerializeField] GameObject bloodSplatterVFX;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public virtual void ProccessInstantEffect(InstantCharacterEffect effect)
    {
       
        effect.ProccesEffect(character);
    }

    public void PlayBloodSplatterVFX(Vector3 contactPoint)
    {
        // if we manualy have placed a blood splatter vfx on this model play its version
        if(bloodSplatterVFX != null)
        {
            GameObject bloodSplatter = Instantiate(bloodSplatterVFX, contactPoint, Quaternion.identity);

        }
        // else use the generic (default version) we have elswere
        else
        {
            GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.bloodSplatterVFX, contactPoint, Quaternion.identity);
        }
    }
}
