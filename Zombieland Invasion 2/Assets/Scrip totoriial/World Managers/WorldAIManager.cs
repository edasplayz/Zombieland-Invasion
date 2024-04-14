using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager instance;

    [Header("Debug")]
    [SerializeField] bool despawnCharacters = false;
    [SerializeField] bool respawnCharacters = false;

    [Header("Characters")]
    [SerializeField] GameObject[] aiCharacters;
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

    private void Start()
    {
        if(NetworkManager.Singleton.IsServer)
        {
            // spawn all ai in scene
            StartCoroutine(WaitForSceneToLoadThenSpawnCharacter());
        }

    }

    private void Update()
    {
        if(respawnCharacters)
        {
            respawnCharacters = false;
            SpawnAllCharacters();
        }

        if(despawnCharacters)
        {
            despawnCharacters = false;
            DespawnAllCharacters();
        }
    }

    private IEnumerator WaitForSceneToLoadThenSpawnCharacter()
    {
        while(!SceneManager.GetActiveScene().isLoaded)
        {
            yield return null;
        }

        SpawnAllCharacters();
    }

    private void SpawnAllCharacters()
    {
        foreach(var character in aiCharacters)
        {
            GameObject instantiatedCharacter = Instantiate(character);
            instantiatedCharacter.GetComponent<NetworkObject>().Spawn();
            spawnedInCharacters.Add(instantiatedCharacter);
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
