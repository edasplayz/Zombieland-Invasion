using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    
    
}

public enum CharacterSlot
{
    CharacterSlot_01,
    CharacterSlot_02,
    CharacterSlot_03,
    CharacterSlot_04,
    CharacterSlot_05,
    CharacterSlot_06,
    CharacterSlot_07,
    CharacterSlot_08,
    CharacterSlot_09,
    CharacterSlot_10,
    NO_SLOT
}

public enum CharacterGroup
{
    Team01,
    Team02,
}

public enum WeaponModelSlot
{
    RightHand,
    LeftHand,
    //right hips
    // left hips
    //back
}

// this is used to calculate damage based on attack type
public enum AttackType
{
    LightAttack01,
    LightAttack02,
    LightAttack03,
    HeavyAttack01,
    HeavyAttack02,
    ChargeAttack01,
    ChargeAttack02,
    RunningAttack01,
    RollingAttack01,
    BackstepAttack01,
}


