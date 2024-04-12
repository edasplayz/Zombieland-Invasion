using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : Item
{
    // animator controller override (change attack animation based on weapon you are currently using)

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

    // weapon guard absorbtions (blocking power)

    [Header("Weapon Poise")]
    public float poiseDamage = 10;
    // ofensive poise bonus when attacking


    [Header("Attack Modifiers")]
    public float light_Attack_01_Modifier = 1.1f;
    // heavy attack modifier
    public float heavy_Attack_01_Modifier = 1.4f;
    public float charge_Attack_01_Modifier = 2.0f;
    // critical damage modifiers ect

    [Header("Stamina Costs Modifiers")]
    public int baseStaminaCost = 20;
    // running attack stamina cost modifier
    // light attack stamina cost modifier
    public float lightAttackStaminaCostMultiplier = 0.9f;
    // heavy attack modiefier 


    // item based action (rb, rt, lb, lt)
    [Header("Actions")]
    public WeaponItemAction oh_RB_Action; // one handed right bumper action
    public WeaponItemAction oh_RT_Action; // one handed right trigger action

    // ash of war

    // blocking sounds


}
