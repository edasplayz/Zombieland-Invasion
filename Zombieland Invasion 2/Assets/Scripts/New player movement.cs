using UnityEngine;
using System.Collections;

public class NewPlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 700f;
    public float jumpForce = 8f;
    public float gravity = 30f;

    public float dashDistance = 5f;
    public float dashCooldown = 2f;
    private bool canDash = true;
    private bool isDashing = false;
    public float dashDuration = 0.2f;
    private float dashTimer = 0f;

    private Rigidbody rb;
    private Vector3 velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleMovementInput();
        HandleDash();
    }

    void HandleMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Rotate player towards the movement direction
        if (direction.magnitude >= 0.1f && !isDashing)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref velocity.y, 0.1f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        // Move the player
        Vector3 moveDirection = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * Vector3.forward;
        rb.velocity = new Vector3(moveDirection.x * speed, rb.velocity.y, moveDirection.z * speed);

        // Handle jumping
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.AddForce(Vector3.up * Mathf.Sqrt(jumpForce * -2f * gravity), ForceMode.VelocityChange);
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f);
    }

    void HandleDash()
    {
        if (Input.GetButtonDown("Dash") && canDash && !isDashing)
        {
            isDashing = true;
            canDash = false;
            dashTimer = 0f;

            // Calculate dash direction based on player's current movement direction
            Vector3 dashDirection = rb.velocity.normalized;
            if (dashDirection.magnitude < 0.1f)
            {
                dashDirection = transform.forward;
            }

            StartCoroutine(Dash(dashDirection));
        }

        if (!canDash)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer >= dashCooldown)
            {
                canDash = true;
            }
        }
    }

    IEnumerator Dash(Vector3 dashDirection)
    {
        float dashTimer = 0f;
        float currentSpeed = speed * 2; // Adjust the dash speed as needed

        while (dashTimer < dashDuration)
        {
            rb.velocity = dashDirection * currentSpeed;
            dashTimer += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }
}
