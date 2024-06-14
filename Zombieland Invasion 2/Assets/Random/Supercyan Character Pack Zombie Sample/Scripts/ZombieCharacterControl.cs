using UnityEngine;

public class ZombieCharacterControl : MonoBehaviour
{
    private enum ControlMode
    {
        /// <summary>
        /// Up moves the character forward, left and right turn the character gradually and down moves the character backwards
        /// </summary>
        Tank,
        /// <summary>
        /// Character freely moves in the chosen direction from the perspective of the camera
        /// </summary>
        Direct,
        FollowPlayer // New control mode for following the player
    }

    [SerializeField] private float m_moveSpeed = 2;
    [SerializeField] private float m_turnSpeed = 200;

    [SerializeField] private Animator m_animator = null;
    [SerializeField] private Rigidbody m_rigidBody = null;

    [SerializeField] private ControlMode m_controlMode = ControlMode.Tank;

    private float m_currentV = 0;
    private float m_currentH = 0;

    private readonly float m_interpolation = 10;

    private Vector3 m_currentDirection = Vector3.zero;





    // New variable for FollowPlayer mode
    [SerializeField] private Transform targetPlayer = null;

    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    private bool isDead = false;
    private bool isAttacking = false;

    private void Awake()
    {
        if (!m_animator) { gameObject.GetComponent<Animator>(); }
        if (!m_rigidBody) { gameObject.GetComponent<Animator>(); }

        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (m_controlMode == ControlMode.FollowPlayer && !isDead && !isAttacking)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.position);

            if (distanceToPlayer <= attackRange)
            {
                // Stop moving towards the player and start attacking
                isAttacking = true;
                m_animator.SetFloat("MoveSpeed", 0f); // Stop movement
                Attack();
            }
            else
            {
                // Continue moving towards the player
                isAttacking = false;
                m_animator.SetFloat("MoveSpeed", m_moveSpeed); // Set movement speed
            }
        }
    }

    private void Attack()
    {
        // Play the attack animation
        m_animator.SetTrigger("Attack");

        // Implement any additional attack logic here
        Debug.Log("Zombie is attacking!");

        // For demonstration purposes, reduce player health
        /*PlayerHealth playerHealth = targetPlayer.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);
        }*/
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        // Play the death animation
        m_animator.SetTrigger("Dead");

        // Disable other components, such as the collider or scripts, to prevent further interactions
        GetComponent<Collider>().enabled = false;
        // Optionally, disable the script to stop updates
        // enabled = false;

        // You might want to play death sounds or particle effects here
    }

    private void FixedUpdate()
    {
        switch (m_controlMode)
        {
            case ControlMode.Direct:
                DirectUpdate();
                break;

            case ControlMode.Tank:
                TankUpdate();
                break;

            case ControlMode.FollowPlayer:
                FollowPlayerUpdate();
                break;
            default:
                Debug.LogError("Unsupported state");
                break;
        }
    }

    private void TankUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
        transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

        m_animator.SetFloat("MoveSpeed", m_currentV);
    }

    private void DirectUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Transform camera = Camera.main.transform;

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

        float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;

        if (direction != Vector3.zero)
        {
            m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

            transform.rotation = Quaternion.LookRotation(m_currentDirection);
            transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

            m_animator.SetFloat("MoveSpeed", direction.magnitude);
        }
    }

    private void FollowPlayerUpdate()
    {
        if (targetPlayer != null)
        {
            // Calculate the direction to the target player
            Vector3 directionToPlayer = (targetPlayer.position - transform.position).normalized;

            // Rotate towards the target player
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), Time.deltaTime * m_turnSpeed);

            // Move towards the target player
            transform.position += directionToPlayer * m_moveSpeed * Time.deltaTime;

            m_animator.SetFloat("MoveSpeed", m_moveSpeed); // You can adjust this based on your animation
        }
        else
        {
            Debug.LogWarning("Target player not assigned for FollowPlayer mode");
        }
    }
}
