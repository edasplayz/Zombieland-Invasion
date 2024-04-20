using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager instance;


    [Header("Characters")]
    [SerializeField] List<AICharacterSpawner> aICharacterSpawners;
    [SerializeField] List<AICharacterManager> spawnedInCharacters;

    [Header("Bosses")]
    [SerializeField] List<AIBossCharacterManager> spawnedInBosses;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }



    public void SpawnCharacter(AICharacterSpawner aICharacterSpawner)
    {
        if(NetworkManager.Singleton.IsServer)
        {
            aICharacterSpawners.Add(aICharacterSpawner);
            aICharacterSpawner.AttemptToSpawnCharacter();
        }
        
    }

    public void AddCharacterToSpawnedCharacterList(AICharacterManager character)
    {
        if(spawnedInCharacters.Contains(character)) 
        { 
            return; 
        }

        spawnedInCharacters.Add(character);

        AIBossCharacterManager bossCharacter = character as AIBossCharacterManager;

        if(bossCharacter != null)
        {
            if(spawnedInBosses.Contains(bossCharacter))
            {
                return;
            }

            spawnedInBosses.Add(bossCharacter);
        }
    }

    public AIBossCharacterManager GetBossCharacterByID(int ID)
    {
        return spawnedInBosses.FirstOrDefault(boss => boss.bossID == ID);
    }
    private void DespawnAllCharacters()
    {
        foreach (var character in spawnedInCharacters)
        {
            character.GetComponent<NetworkObject>().Despawn();
        }
    }

    private void DisableAllCharacters()
    {
        // to do disable characters sync disable status on network
        // disable gameobjects for clients upon connecting if disabled status is true 
        // can be used to disable characters that are far from players to save memory
        // characters can be split into areas (area 1 area 2 ...)
    }
}
