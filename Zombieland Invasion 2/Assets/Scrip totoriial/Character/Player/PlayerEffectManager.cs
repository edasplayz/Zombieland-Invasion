using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectManager : CharacterEffectsManager
{
    [Header("Debug Delete Later")]
    [SerializeField] InstantCharacterEffect effectToTest;
    [SerializeField] bool processEffect = false;

    private void Update()
    {
        if (processEffect)
        {
            processEffect = false;
            // when we instantiate it, the original is not effected 
            // why are we instantiating a copy of this, insted of just using it as it is?
            TakeStaminaDamageEffect effect = Instantiate(effectToTest) as TakeStaminaDamageEffect;
            effect.staminDamage = 55;

            // when we dont instantiate it, the original is changed (you do not want this in most cases)
            //TakeStaminaDamageEffect effect = Instantiate(effectToTest) as TakeStaminaDamageEffect;
            //effect.staminDamage = 55;
            ProccessInstantEffect(effect);
        }
        


    }
}
