using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIKnightCharacterManager : AIBossCharacterManager
{

    // boss need to have his own sounds and attacks

    [HideInInspector] public AIKnightSoundFXManager knightSoundFXManager;
    [HideInInspector] public AIKnightCombatManager knightCombatManager;

    protected override void Awake()
    {
        base.Awake();
        knightSoundFXManager = GetComponent<AIKnightSoundFXManager>();
        knightCombatManager = GetComponent<AIKnightCombatManager>();
        
    }
}
