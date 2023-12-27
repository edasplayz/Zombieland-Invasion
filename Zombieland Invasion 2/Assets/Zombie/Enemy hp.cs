using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Enemyhp : MonoBehaviour
{
    private Animator animator;
    private ZombieAI zombieAi; // Reference to the ZombieAi script component
    [Header("Stats")]
    public int Health;

    private void Start()
    {
        animator = GetComponent<Animator>();
        zombieAi = GetComponent<ZombieAI>(); // Get the ZombieAi script component
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("Dead");

        // Disable the ZombieAi script component when the enemy dies
        if (zombieAi != null)
        {
            zombieAi.enabled = false;
        }
    }
}
