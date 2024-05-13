using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGameSessionManager : MonoBehaviour
{
    public static WorldGameSessionManager Instance;

    [Header("Active Players In Session")]
    public List<PlayerManager> players = new List<PlayerManager>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddPlayerToActivePlayersList(PlayerManager player)
    {
        // check the list if it does not already contain the player, add them 
        if (!players.Contains(player))
        {
            players.Add(player);
        }

        // check the list for null slots and remove the null slots

        for(int i = players.Count - 1; i > -1; i--)
        {
            if (players[i] == null)
            {
                players.RemoveAt(i);
            }
        }
    }

    public void RemovePlayerFromActivePlayersList(PlayerManager player)
    {
        // check the list if it does contain the player, remove them 
        if (players.Contains(player))
        {
            players.Remove(player);
        }
        // check the list for null slots and remove the null slots
        for (int i = players.Count - 1; i > -1; i--)
        {
            if (players[i] == null)
            {
                players.RemoveAt(i);
            }
        }
    }
}
