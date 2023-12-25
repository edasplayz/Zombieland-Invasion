using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class ZombieAI : MonoBehaviour
{
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;
   // [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float standStillTime = 3f;
    [SerializeField] private float moveRandomlyTime = 2f;
    [SerializeField] private float wanderRange = 5f; // Added range for wandering

    private Animator animator;
    private Transform player;
    private bool isPlayerInRange = false;
    private bool isMovingRandomly = false;
    private float timer = 0f;

    private float moveSpeedParameter = 0f; // Added parameter for moveSpeed
    private bool playerEnteredDuringRandomMove = false;

    NavMeshAgent navMeshAgent;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        timer = Random.Range(0f, moveRandomlyTime);
    }

    void Update()
    {
        CheckPlayerInRange();

        if (isPlayerInRange)
        {
            // Player is in range, move towards the player
            MoveTowardsPlayer();

            // Check if the player is close enough to attack
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                Attack();
            }
            else
            {
                // Player is not close enough to attack, trigger movement animation
                PlayWalkAnimation();
            }
        }
        else
        {
            // Player is not in range
            if (!isMovingRandomly)
            {
                timer += Time.deltaTime;
                if (timer >= standStillTime)
                {
                    // Stand still for a certain time
                    StandStill();

                    if (timer >= standStillTime + moveRandomlyTime)
                    {
                        // Move randomly for a certain time
                        MoveRandomly();
                        timer = 0f;
                    }
                }
                else
                {
                    // Check if the zombie is stationary and trigger idle animation
                    if (navMeshAgent.velocity.magnitude < 0.5f)
                    {
                        PlayIdleAnimation();
                    }
                }
            }
        }
    }

    void PlayIdleAnimation()
    {
        // Trigger idle animation
        SetMoveSpeed(0f);
    }

    void CheckPlayerInRange()
    {
        // Check if the player is within detection range
        bool playerInRange = Vector3.Distance(transform.position, player.position) <= detectionRange;
        isPlayerInRange = playerInRange;
    }


    void MoveTowardsPlayer()
    {
        // Rotate towards the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        // Set the destination for the NavMeshAgent
        navMeshAgent.SetDestination(player.position);

        // Check if the zombie is moving
        if (!navMeshAgent.isStopped)
        {
            // Trigger movement animation
            SetMoveSpeed(1f);
        }
        else
        {
            // Stop moving animation if the zombie is not moving
            SetMoveSpeed(0f);
        }
    }

    void PlayWalkAnimation()
    {
        // Check if the zombie is moving
        if (navMeshAgent.velocity.magnitude > 0.1f)
        {
            // Trigger movement animation
            SetMoveSpeed(1f);
        }
        else
        {
            // Stop moving animation if the zombie is not moving
            SetMoveSpeed(0f);
        }
    }


    void StandStill()
    {
        // Stand still and trigger idle animation
        //SetMoveSpeed(0f);
        gameObject.GetComponent<NavMeshAgent>().isStopped = false;
    }

    void MoveRandomly()
    {
        // Move randomly and trigger movement animation
        Vector3 randomDirection = Random.onUnitSphere * wanderRange;
        randomDirection.y = 0f; // Ensure the zombie moves only on the horizontal plane

        // Calculate a random destination point within the specified range
        Vector3 randomDestination = transform.position + randomDirection;

        // Use NavMeshAgent to move the zombie
        navMeshAgent.SetDestination(randomDestination);

        // Trigger movement animation
        SetMoveSpeed(1f);

        // Start checking for player proximity while moving randomly
        StartCoroutine(CheckPlayerProximity(randomDestination));
    }

    IEnumerator CheckPlayerProximity(Vector3 randomDestination)
    {
        // Check for player proximity while moving randomly
        while (!isPlayerInRange && Vector3.Distance(transform.position, player.position) > detectionRange)
        {
            // Check if the zombie has reached its destination
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                // Stop moving animation when the zombie has reached its destination
                SetMoveSpeed(0f);
            }

            yield return null;
        }

        // Player is in range
        if (isPlayerInRange)
        {
            // Stop moving randomly
            StopMovingRandomly();

            // Move towards the player immediately
            MoveTowardsPlayer();
        }
        else
        {
            // Player is not in range, continue moving randomly
            StartCoroutine(WaitForDestination());
        }
    }

    void StopMovingRandomly()
    {
        // Stop moving randomly
        isMovingRandomly = false;
        SetMoveSpeed(0f); // Set the move speed to 0 to stop the movement
    }

    IEnumerator WaitForDestination()
    {
        // Store the initial position
        Vector3 initialPosition = transform.position;

        // Wait until the zombie reaches its destination or the player comes into range
        while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            // If the player comes into range during random movement, break from the loop
            if (isPlayerInRange)
            {
                playerEnteredDuringRandomMove = true;
                break;
            }
            yield return null;
        }

        // If the player didn't come into range during random movement, stop moving randomly
        if (!playerEnteredDuringRandomMove)
        {
            isMovingRandomly = false;
            SetMoveSpeed(0f);
        }

        // Reset the flag
        playerEnteredDuringRandomMove = false;
    }


    void Attack()
    {
        // Trigger attack animation
        animator.SetTrigger("Attack");

        // Implement attack logic here if needed
    }

    public void Die()
    {
        // Trigger dead animation
        animator.SetTrigger("Dead");

        // Implement any additional logic for when the zombie dies
    }

    void SetMoveSpeed(float speed)
    {
        // Set the "MoveSpeed" parameter in the Animator
        moveSpeedParameter = speed;
        animator.SetFloat("MoveSpeed", moveSpeedParameter);
    }


}
