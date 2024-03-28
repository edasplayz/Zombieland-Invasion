using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Mathematics;
using UnityEngine;

public class CharacterStatManager : MonoBehaviour
{
    CharacterManager character;
    [Header("Stamina Regeneration")]
    [SerializeField] float staminaRegenerationAmount = 2;
    private float staminaRegenerationTimer = 0;
    private float staminaTickTimer = 0;
    [SerializeField] float staminaRegenerationDelay = 2;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
    {
        float stamina = 0;

        // create an equation for how you want your stamina to be calculated

        stamina = endurance * 10;
        return Mathf.RoundToInt(stamina);
    }

    public virtual void RegenerateStamina()
    {
        // only owners can edit they network variebles
        if (!character.IsOwner)
        {
            return;
        }

        //we do not want to regenerate stamina if we are using it
        if (character.characterNetworkManager.isSprinting.Value)
        {
            return;
        }

        if (character.isPreformingAction)
        {
            return;
        }

        staminaRegenerationTimer += Time.deltaTime;

        if (staminaRegenerationTimer >= staminaRegenerationDelay)
        {
            if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
            {
                staminaTickTimer += Time.deltaTime;

                if (staminaTickTimer >= 0.1)
                {
                    staminaTickTimer = 0;
                    character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                }
            }
        }
    }

    public virtual void ResetStaminaRegenTimer(float prewiusStaminaAmount, float currentStaminaAmount)
    {
        // we only want to reset the regeneration if the action used stamina
        // we dont want to reset the regeneration if we already regenerating stamina
        if(currentStaminaAmount < prewiusStaminaAmount)
        {
            staminaRegenerationTimer = 0;
        }
        
    }
}
