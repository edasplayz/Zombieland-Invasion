using UnityEngine;
using TMPro;

public class SpeedTracker : MonoBehaviour
{
    public Rigidbody playerRb; // Assign the player's Rigidbody in the Inspector
    public TextMeshProUGUI speedText;

    // Update is called once per frame
    void Update()
    {
        if (playerRb != null)
        {
            float speed = playerRb.velocity.magnitude;
            speedText.text = "Speed: " + speed.ToString("F2") + " m/s"; // Format speed to two decimal places
        }
    }
}
