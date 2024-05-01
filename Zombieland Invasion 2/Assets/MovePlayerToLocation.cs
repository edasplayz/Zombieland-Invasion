using UnityEngine;

public class MovePlayerToLocation : MonoBehaviour
{
    public Transform targetLocation; // Target location where you want to move the players

    public void MovePlayersToTargetLocation()
    {
        // Get reference to the WorldGameSessionManager instance
        WorldGameSessionManager sessionManager = WorldGameSessionManager.Instance;

        // Check if session manager instance is available
        if (sessionManager != null)
        {
            // Get the list of active players from the session manager
            foreach (PlayerManager player in sessionManager.players)
            {
                // Check if player exists and has a target location
                if (player != null && targetLocation != null)
                {
                    // Move the player to its target location
                    player.transform.position = targetLocation.position;
                }
            }
        }
        else
        {
            Debug.LogError("WorldGameSessionManager instance is not available!");
        }
    }
}
