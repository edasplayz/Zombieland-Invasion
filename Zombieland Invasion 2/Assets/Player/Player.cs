using UnityEngine;

public class Player : MonoBehaviour
{
    // Singleton instance
    public static Player Instance { get; private set; }

    // Player Stats
    public int maxHealth = 100;
    public int currentHealth;

    public int attackDamage = 10;

    public float moveSpeed = 5f;

    public int maxDashCount = 2;
    public int currentDashCount;

    public float dashRechargeRate = 1f;

    public float protection = 0f; // Represented as a percentage (0% to 100%)

 
}
