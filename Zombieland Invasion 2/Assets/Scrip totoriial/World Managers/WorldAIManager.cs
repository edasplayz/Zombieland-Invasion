using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager instance;


    [Header("Characters")]
    [SerializeField] List<AICharacterSpawner> aICharacterSpawners;
    [SerializeField] List<GameObject> spawnedInCharacters;

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
