using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Enemyhp : MonoBehaviour
{
    private Animator animator;
    private ZombieAI zombieAi; // Reference to the ZombieAi script component
    [Header("Stats")]
    public float Health;
    public float CurrentHealth;
    [SerializeField] private HeathBar heathBar; // pasiekimas prieso gyvybiu eilutei

    private void Start()
    {

        CurrentHealth = Health; // nustatomos dabartin?s gyvybes
        heathBar.UpdateHeathBar(Health, CurrentHealth); // atnaujinama gyvybiu eilute
        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        zombieAi = GetComponent<ZombieAI>(); // Get the ZombieAi script component
    }
    private void Update()
    {
        heathBar.UpdateHeathBar(Health, CurrentHealth); // atnaujinama gyvybiu eilute
    }
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

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
