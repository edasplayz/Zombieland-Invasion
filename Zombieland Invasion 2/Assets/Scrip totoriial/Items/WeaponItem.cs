using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : Item
{
    // animator controller override (change attack animation based on weapon you are currently using)
    [Header("Animations")]
    public AnimatorOverrideController weaponAnimator;

    [Header("Model Instantiation")]
    public WeaponModelType weaponModelType;

    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("Weapon Requirements")]
    public int streangthREQ = 0;
    public int dexREQ = 0;
    public int intREQ = 0;
    public int faitREQ = 0;

    [Header("Weapon Based Damage")]
    public int physicalDamage = 0;
    public int magicDamage = 0;
    public int fireDamage = 0;
    public int holyDamage = 0;
    public int lightningDamage = 0;

    [Header("Animation Speed Modifiers")]
    public float light_Attack_Speed_Modifier = 1.0f;
    public float running_Attack_Speed_Modifier = 1.0f;
    public float rolling_Attack_Speed_Modifier = 1.0f;
    public float backstep_Attack_Speed_Modifier = 1.0f;

    // weapon guard absorbtions (blocking power)

    [Header("Weapon Poise")]
    public float poiseDamage = 10;
    // ofensive poise bonus when attacking


    [Header("Attack Modifiers")]
    public float light_Attack_01_Modifier = 1.0f;
    public float light_Attack_02_Modifier = 1.2f;
    public float light_Attack_03_Modifier = 1.4f;
    // heavy attack modifier
    public float heavy_Attack_01_Modifier = 1.4f;
    public float heavy_Attack_02_Modifier = 1.6f;

    public float charge_Attack_01_Modifier = 2.0f;
    public float charge_Attack_02_Modifier = 2.2f;

    public float running_Attack_01_Modifier = 1.1f;

    public float rolling_Attack_01_Modifier = 1.1f;

    public float backstep_Attack_01_Modifier = 1.1f;
    // critical damage modifiers ect

    [Header("Stamina Costs Modifiers")]
    public int baseStaminaCost = 20;
    // running attack stamina cost modifier
    // light attack stamina cost modifier
    public float lightAttackStaminaCostMultiplier = 1f;
    public float heavyAttackStaminaCostMultiplier = 1.3f;
    public float chargeAttackStaminaCostMultiplier = 1.5f;
    public float runningAttackStaminaCostMultiplier = 1.1f;
    public float rollingAttackStaminaCostMultiplier = 1.1f;
    public float backstepAttackStaminaCostMultiplier = 1.1f;
    // heavy attack modiefier 

    [Header("Weapon Blocking Absorbtion")]
    public float physicalBaseDamageAbsorption = 50;
    public float magicalBaseDamageAbsorption = 50;
    public float fireBaseDamageAbsorption = 50;
    public float holyBaseDamageAbsorption = 50;
    public float lightningBaseDamageAbsorption = 50;
    public float stability = 50; // reduces stamina lost from block


    // item based action (rb, rt, lb, lt)
    [Header("Actions")]
    public WeaponItemAction oh_RB_Action; // one handed right bumper action
    public WeaponItemAction oh_RT_Action; // one handed right trigger action
    public WeaponItemAction oh_LB_Action; // one handed left bumper action

    // ash of war

    // blocking sounds

    [Header("SFX")]
    public AudioClip[] blocking;
    public AudioClip[] whooshes;
}
