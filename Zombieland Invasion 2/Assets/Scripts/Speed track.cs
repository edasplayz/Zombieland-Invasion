using UnityEngine;
using TMPro;
using System.Collections;
public class SpeedTracker : MonoBehaviour
{
    public Rigidbody playerRb; // Assign the player's Rigidbody in the Inspector
    public TextMeshProUGUI speedText;
    
    public PlayerMovement playerMovement; // Reference to your PlayerMovement script

    public TMP_Text countdownText;
    // Set the initial cooldown duration
    private float cooldownDuration = 5f; // Example: Set your desired cooldown duration in seconds




    // Update is called once per frame
    private void Start()
    {
        cooldownDuration = playerMovement.dashCooldown;
        // Example: Initialize the countdown text
        
    }

    void Update()
    {
        if (playerRb != null)
        {
            float speed = playerRb.velocity.magnitude;
            speedText.text = "Speed: " + speed.ToString("F2") + " m/s"; // Format speed to two decimal places
            
        }
        // Check if the cooldownText and playerMovement are assigned
        
    }
    // Coroutine for the countdown
    public void StartCountdown(float countdownDuration)
    {
        // Example: Initialize the countdown text
        UpdateCountdownText(countdownDuration);

        // Example: Start the countdown coroutine
        StartCoroutine(StartCooldown(countdownDuration));
    }
    private IEnumerator StartCooldown(float countdownDuration)
    {
        float timer = countdownDuration;

        // Countdown loop
        while (timer > 0f)
        {
            // Update the countdown text
            UpdateCountdownText(timer);

            // Decrease the timer
            timer -= Time.deltaTime;

            yield return null;
        }

        // Ensure the countdown ends with a display of 0 seconds
        UpdateCountdownText(0f);
    }
    private void UpdateCountdownText(float timeRemaining)
    {
        // Example: Format the timeRemaining to display as seconds with one decimal place
        string countdownString = string.Format("Cooldown: {0:F1}s", timeRemaining);

        // Update the TextMeshPro Text component to display the countdown
        countdownText.text = countdownString;
    }

    public void GetTimer(float timer)
    {
        cooldownDuration = timer;
        StartCountdown(cooldownDuration);
    }

    


}
