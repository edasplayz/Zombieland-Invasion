using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    // process instant effects (take damage, heal)

    // proccess timed effect (poison, build ups)

    // process tatic effect (armor, removing buffs from talisman ect)

    CharacterManager character;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public virtual void ProccessInstantEffect(InstantCharacterEffect effect)
    {
        // take in an effect 
        // process it 
        effect.ProccesEffect(character);
    }
}
