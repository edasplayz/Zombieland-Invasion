using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player; // Player object

    public LayerMask whatIsGround, whatIsPlayer; // Layers for ground and player detection

    public float health; // Enemy's health

    public GameObject projectilePrefab; // Projectile object that the enemy will shoot
    public float projectileForce = 10f; // Projectile flight strength
    public GameObject emitter; // Location from which the projectile will be shot

    // Patrolling
    public Vector3 walkPoint; // Point where the enemy will search for the player
    bool walkPointSet;
    public float walkPointRange; // Range within which the enemy will search for the player

    // Attacking
    public float timeBetweenAttack; // How often the enemy can shoot
    float timeSinceLastShot = 0f; // Time passed since the last shot

    bool alreadyAttacked; // Is the enemy currently attacking

    // States
    public float sightRange, attackRange; // Distance at which the enemy can detect and attack the player
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform; // Find the player
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Check for sight and attack range 
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

        timeSinceLastShot += Time.deltaTime; // Increase the time since the enemy last shot
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            if (timeSinceLastShot >= timeBetweenAttack)
            {
                ShootProjectile();
                timeSinceLastShot = 0f;
            }
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) DestroyEnemy();
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    void ShootProjectile()
    {
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        Vector3 position = emitter.transform.position;
        Quaternion rotation = emitter.transform.rotation;
        GameObject projectile = Instantiate(projectilePrefab, position, rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(direction * projectileForce, ForceMode.Impulse);
    }
}
