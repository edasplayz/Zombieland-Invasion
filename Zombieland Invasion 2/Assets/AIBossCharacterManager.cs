using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AIBossCharacterManager : AICharacterManager
{
    // GIVE THIS A,I a unique if
    public int bossID = 0;
    [SerializeField] bool hasBeenDefeated = false;
    // when this ai is spawned check our save file (dictonary)
    // if the save file does not contain a boss monster with this i.d add it
    // if it is present cheeck if boss has been defeted
    // if the boss has been defeted disable the game object
    // if the boss has not been defeted allow this object to continue to be active


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // if this is the host world
        if(IsServer)
        {
            // if our save data does not contain information on this boss add it now 
            if(!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, false);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, false);
            }
            // otherwise load the data that already exist on this boss
            else
            {
                hasBeenDefeated = WorldSaveGameManager.instance.currentCharacterData.bossesDefeated[bossID];

                if(hasBeenDefeated )
                {
                    aICharacterNetworkManager.isActive.Value = false;
                }
            }
        }
    }

    

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        if (IsOwner)
        {
            characterNetworkManager.currentHealth.Value = 0;
            isDead.Value = true;

            // reset any flags here that need to be reset 
            // nothing yet

            // if we are not grounded, play an aerial death animation

            if (!manuallySelectDeathAnimation)
            {
                characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
            }

            hasBeenDefeated = true;

            // if our save data does not contain information on this boss add it now 
            if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
            }
            // otherwise load the data that already exist on this boss
            else
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Remove(bossID);
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);

            }

            WorldSaveGameManager.instance.SaveGame();
        }

        

        // play some death sfx

        yield return new WaitForSeconds(5);

        // award players with runes

        // disable character 
    }
}

