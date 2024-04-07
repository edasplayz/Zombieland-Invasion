using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ZombieAI : MonoBehaviour
{
    [SerializeField] private float visionRadius = 10f;
    [SerializeField] private float smallVisionRadius = 3f;
    [SerializeField, Range(1f, 180f)] private float visionAngle = 45f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDelay = 2f;
    [SerializeField] private float turningSpeed = 120f; // Adjust this value as needed

    private Transform player;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Vector3 lastKnownPlayerPosition;
    private bool canAttack = true;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        lastKnownPlayerPosition = transform.position;

        // Set turning speed
        navMeshAgent.angularSpeed = turningSpeed;
    }

    private void Update()
    {
        // Check if the player is in the vision cone or the small vision zone
        if (CanSeePlayer() || IsPlayerInSmallVisionZone())
        {
            // Player is in sight
            MoveTowardsPlayer();

            // Check if the player is in attack range and can attack
            if (Vector3.Distance(transform.position, player.position) <= attackRange && canAttack)
            {
                Attack();
            }
        }
        else
        {
            // Player is not in sight, move to the last known position
            MoveToLastKnownPosition();
        }

        // Update animator parameters based on movement
        float moveSpeed = navMeshAgent.velocity.magnitude / navMeshAgent.speed;
        animator.SetFloat("MoveSpeed", moveSpeed);

        // Update animator parameters based on attack state
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool isAttacking = stateInfo.IsTag("Attack");
        //animator.SetBool("IsAttacking", isAttacking);

        // Log the current animation state
        // Debug.Log("Current Animation State: " + stateInfo.fullPathHash);
    }

    private bool CanSeePlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer <= visionAngle / 2f)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, directionToPlayer, out hit, visionRadius))
            {
                if (hit.collider.CompareTag("Player") && IsPlayerVisible(hit.collider.gameObject))
                {
                    // Player is in sight
                    DrawVisionCone(Color.green);
                    lastKnownPlayerPosition = player.position; // Update last known position
                    return true;
                }
            }
        }

        // Player is not in sight
        return false;
    }

    private bool IsPlayerInSmallVisionZone()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        RaycastHit hit;

        // Perform a raycast within the small vision radius
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, smallVisionRadius))
        {
            if (hit.collider.CompareTag("Player") && IsPlayerVisible(hit.collider.gameObject))
            {
                // Player is in small vision zone and visible, update last known position
                lastKnownPlayerPosition = player.position;
                return true;
            }
        }

        return false;
    }

    private bool IsPlayerVisible(GameObject playerObject)
    {
        // Check if the player is visible, e.g., not behind a wall
        // You can customize this logic based on your game's requirements
        // For a simple example, you can use layers or additional tags for obstacles
        return !Physics.Linecast(transform.position, player.position, LayerMask.GetMask("Obstacle"));
    }

    private void MoveTowardsPlayer()
    {
        // Set the destination for the NavMeshAgent
        navMeshAgent.SetDestination(player.position);
    }

    private void MoveToLastKnownPosition()
    {
        // Set the destination for the NavMeshAgent to the last known player position
        navMeshAgent.SetDestination(lastKnownPlayerPosition);
    }

    private void Attack()
    {
        // Implement attack logic here
        animator.SetTrigger("Attack");
        Debug.Log("Attacking player!");

        // Set canAttack to false to prevent further attacks during the delay
        canAttack = false;

        // Start the coroutine to reset canAttack after the delay
        StartCoroutine(ResetAttackDelay());
    }

    private IEnumerator ResetAttackDelay()
    {
        yield return new WaitForSeconds(attackDelay);

        // Reset canAttack to true after the delay
        canAttack = true;
    }

    private void DrawVisionCone(Color color)
    {
        Vector3 leftConeDirection = Quaternion.Euler(0, -visionAngle / 2f, 0) * transform.forward;
        Vector3 rightConeDirection = Quaternion.Euler(0, visionAngle / 2f, 0) * transform.forward;

        Debug.DrawLine(transform.position, transform.position + leftConeDirection * visionRadius, color);
        Debug.DrawLine(transform.position, transform.position + rightConeDirection * visionRadius, color);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the main vision radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);

        // Draw the small vision zone
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, smallVisionRadius);

        // Draw the vision cone during editing
        DrawVisionCone(Color.blue);
    }
}
