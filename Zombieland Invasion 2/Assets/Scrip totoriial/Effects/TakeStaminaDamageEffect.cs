using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Craracter Effects/Instant Effects/Take Stamina Damage")]
public class TakeStaminaDamageEffect : InstantCharacterEffect
{
    

    public float staminDamage;
    public override void ProccesEffect(CharacterManager character)
    {

        //base.ProccesEffect(character);
        CalculateStaminaDamage(character);
    }

    private void CalculateStaminaDamage(CharacterManager character)
    {
        // compare the base stamina agoinst other player effects/modifiers
        // change the value before subtracking/adding it 
        // play sound fx of vfx during effect 

        if(character.IsOwner)
        {
            Debug.Log("Character is taking: " + staminDamage + " stamina damage");
            character.characterNetworkManager.currentStamina.Value -= staminDamage;
        }
    }
}
