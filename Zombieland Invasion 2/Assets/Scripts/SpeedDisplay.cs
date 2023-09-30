using UnityEngine;
using TMPro;

public class SpeedDisplay : MonoBehaviour
{
    public TMP_Text speedText;
    public MovementScript movementScript; // Reference to your movement script (assuming it has the speed information)

    private void Update()
    {
        // Check if the speedText and movementScript are assigned
        if (speedText != null && movementScript != null)
        {
            // Get the player's current speed from the MovementScript.
            float playerSpeed = movementScript.rb.velocity.magnitude;

            // Format the speed value to display with one decimal place
            string speedString = string.Format("Speed: {0:F1}", playerSpeed);

            // Update the TextMeshPro Text component to display the speed
            speedText.text = speedString;
        }
    }
}
