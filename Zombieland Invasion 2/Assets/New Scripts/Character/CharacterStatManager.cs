using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterStatManager : MonoBehaviour
{
    CharacterManager character;
    [Header("Stamina Regeneration")]
    [SerializeField] float staminaRegenerationAmount = 2;
    private float staminaRegenerationTimer = 0;
    private float staminaTickTimer = 0;
    [SerializeField] float staminaRegenerationDelay = 2;

    [Header("Blocking Absorbtions")]
    public float blockingPhysicalAbsobtion;
    public float blockingFireAbsobtion;
    public float blockingMagicAbsobtion;
    public float blockingLightningAbsobtion;
    public float blockingHolyAbsobtion;
    public float blockingStability;

    [Header("Poise")]
    public float totalPoiseDamage; // how mutch poise damage we have taken
    public float offensivePoiseBonus; // the poise bonus gained from using weapon (heavy weapon have mutch larger bonus  to do)
    public float basePoiseDefense; // the poise bunus gained from armor/ talisman to do
    public float defaultPoiseResetTime = 8; // the time it taks for poise damaga to reset (must not be hit in the time of it will reset)
    public float poiseResetTimer = 0; // the current timer for poise reset


    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandlePoiseResetTimer();
    }

    public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
    {
        float stamina = 0;

        

        stamina = endurance * 10;
        return Mathf.RoundToInt(stamina);
    }

    public int CalculateHealthBasedOnVitalityLevel(int vitality)
    {
        float health = 0;

        

        health = vitality * 15;
        return Mathf.RoundToInt(health);
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

    protected virtual void HandlePoiseResetTimer()
    {
        if(poiseResetTimer > 0)
        {
            poiseResetTimer -= Time.deltaTime;
        }
        else
        {
            totalPoiseDamage = 0;
        }
    }
}
