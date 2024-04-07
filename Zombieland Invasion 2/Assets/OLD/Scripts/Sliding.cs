using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObject;
    private Rigidbody rb;
    private MovementScript pm;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    public float slideStartForce;
    public float slideDamping;
    private float slideTimer;
    public float slideSpeedDecreaseRate = 0.1f;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    private Vector3 slideDirection;

    private bool slidingEnded;

    public float slideToCrouchSpeedThreshold;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<MovementScript>();

        startYScale = playerObject.localScale.y;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Start sliding if the slide key is pressed and there is movement input.
        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0) && !pm.sliding)
            StartSlide();

        // Stop sliding if the slide key is released while sliding.
        if (Input.GetKeyUp(slideKey) && pm.sliding)
            StopSlide();
    }

    private void StartSlide()
    {
        pm.sliding = true;
        slidingEnded = false;

        playerObject.localScale = new Vector3(playerObject.localScale.x, slideYScale, playerObject.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);


        // Stop vertical movement by setting Y velocity to zero.
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Apply an initial force in the slide direction to make it smoother.
        slideDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(slideDirection.normalized * slideStartForce, ForceMode.Force);



        slideTimer = maxSlideTime;
    }

    private void FixedUpdate()
    {
        if (pm.sliding)
        {
            SlideMovement();

            // Check if sliding time has ended.
            if (slideTimer <= 0 && !slidingEnded)
            {
                slidingEnded = true;

                // Change the state to crouching when sliding ends.
                pm.sliding = false;
                if (rb.velocity.magnitude <= slideToCrouchSpeedThreshold)
                {
                    pm.state = MovementScript.MovementState.crounching;
                    playerObject.localScale = new Vector3(playerObject.localScale.x, pm.crounchYScale, playerObject.localScale.z);
                }
            }
        }
    }

    private void SlideMovement()
    {
        // Calculate the movement direction on the ground plane.
        Vector3 moveDirection = Vector3.ProjectOnPlane(rb.velocity, Vector3.up).normalized;

        // Calculate the lateral (sideways) velocity.
        Vector3 lateralVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Calculate the force required to completely stop lateral movement.
        Vector3 lateralForce = -lateralVelocity;

        // Check if we're on a slope.
        bool onSlope = pm.OnSlope();

        // Gradually decrease speed on flat surface.
        if (!onSlope)
        {
            pm.moveSpeed -= slideSpeedDecreaseRate * Time.deltaTime;
            pm.moveSpeed = Mathf.Max(pm.moveSpeed, 0f); // Ensure moveSpeed doesn't go negative.
        }

        // Apply the slide force in the calculated direction.
        rb.AddForce(moveDirection * slideForce, ForceMode.Force);

        // Apply the lateral force to completely stop sideways movement.
        rb.AddForce(lateralForce, ForceMode.Force);

        slideTimer -= Time.deltaTime;

        // Stop sliding if the slide timer runs out.
        if (slideTimer <= 0)
        {
            StopSlide();
        }
    }

    private void StopSlide()
    {
        pm.sliding = false;
        playerObject.localScale = new Vector3(playerObject.localScale.x, startYScale, playerObject.localScale.z);
    }
}
