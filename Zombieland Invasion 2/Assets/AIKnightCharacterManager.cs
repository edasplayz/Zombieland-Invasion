using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIKnightCharacterManager : AIBossCharacterManager
{

    // WHY GIVE DURK HIS OWN CHARACTER MANAGER?
    // OUR CHARACTER MANAGERS ACT AS A HUB TO WHERE WE CAN REFERENCE ALL COMPONENTS OF A CHARACTER
    // A "PLAYER" MANAGER FOR EXAMPLE WILL HAVE ALL THE UNIQUE COMPONENTS OF A PLAYER CHARACTER
    // AN "UNDEAD" WILL HAVE ALL THE UNIQUE COMPONENTS OF AN UNDEAD
    // SINCE KNIGHT HAS HIS OWN SFX (SWORD SWING) THAT ARE UNIQUE TO HIS CHARACTER ONLY, WE CREATED A KNIGHT SFX MANAGER
    // AND TO REFERENCE THIS NEW SFX MANAGER, AND TO KEEP OUR DESIGN PATTERN THE SAME, WE NEED A KNIGHT CHARACTER MANAGER TO REFERENCE IT FROM

    [HideInInspector] public AIKnightSoundFXManager knightSoundFXManager;
    [HideInInspector] public AIKnightCombatManager knightCombatManager;

    protected override void Awake()
    {
        base.Awake();
        knightSoundFXManager = GetComponent<AIKnightSoundFXManager>();
        knightCombatManager = GetComponent<AIKnightCombatManager>();
        
    }
}
