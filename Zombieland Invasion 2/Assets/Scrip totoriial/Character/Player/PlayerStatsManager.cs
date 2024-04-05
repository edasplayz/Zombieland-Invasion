using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatManager
{
    PlayerManager player;
    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();

        
    }

    protected override void Start()
    {
        base.Start();

        // when whe make a character creacion meniu, and set the stats depending on the class, this will be calculated there 
        // until then howewer, stats are never calculated, se we do it here on start, if a save file exists they will be over written when loading into a scene
        CalculateHealthBasedOnVitalityLevel(player.playerNetworkManager.vitality.Value);
        CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);
    }

}
