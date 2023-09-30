using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float walkSpeed;
    public float SprintSpeed;
    public float slideSpeed;

    public float desiredMoveSpeed;
    public float lastDesiredMoveSpeed;


    public float groundDrag;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplayer;
    bool readyToJump;

    [Header("Crounching")]
    public float crounchSpeed;
    public float crounchYScale;
    private float startYscale;
    private bool isCrouching = false;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask WhatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit SlopeHit;
    private bool exitingSlope;

    [Header("Sliding")]
    public float slideSpeedDecreaseRate; // Rate at which sliding speed decreases
    private bool slidingSpeedDecreasing;


    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    public Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crounching,
        sliding,
        air
    }

    public bool sliding;

    private void Start()
    {
        readyToJump = true;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startYscale = transform.localScale.y;
        slidingSpeedDecreasing = false;
    }

    private void Update()
    {
        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, WhatIsGround);
        MyInput();
        SpeedControl();
        StateHandler();

        isCrouching = Input.GetKey(crouchKey);

        //handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        if (sliding && moveSpeed > crounchSpeed)
        {
            slidingSpeedDecreasing = true;
            moveSpeed -= slideSpeedDecreaseRate * Time.deltaTime;

            // If speed reaches or goes below crouching speed, change state to crouching
            if (moveSpeed <= crounchSpeed)
            {
                moveSpeed = crounchSpeed;
                slidingSpeedDecreasing = false;
                state = MovementState.crounching;
            }
        }
        else
        {
            slidingSpeedDecreasing = false;
        }
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        //when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //start crounch
        
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crounchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        
        // stop crounching
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYscale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        //move - sliding
        if (sliding)
        {
            state = MovementState.sliding;

            if (OnSlope() && rb.velocity.y < 0.1f)
                moveSpeed = slideSpeed;
            else
                moveSpeed = SprintSpeed; // Instant speed change here
        }

        //mode - crouching
        else if (Input.GetKey(crouchKey))
        {
            state = MovementState.crounching;
            moveSpeed = crounchSpeed;
        }

        // mode - sprinting
        else if (grounded && Input.GetKey(sprintKey) && !Input.GetKey(crouchKey))
        {
            state = MovementState.sprinting;
            moveSpeed = SprintSpeed; // Instant speed change here
        }

        // mode - walking
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        // mode - air
        else
        {
            state = MovementState.air;
        }

        // No need to check for a drastic change; just set the desired move speed directly
        lastDesiredMoveSpeed = moveSpeed;
    }


    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        //smoothly lerp moveSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while(time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            time += Time.deltaTime;
            yield return null;
        }
        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        //calculate mevemnt diretion 
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        //on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);
            if(rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        //on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        //in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplayer, ForceMode.Force);

        // turn gravity off while on slope 
        // rb.useGravity = !OnSlope();

        if (sliding && (!slidingSpeedDecreasing || moveSpeed <= crounchSpeed))
        {
            sliding = false;
        }

    }

    private void SpeedControl()
    {
        //limit speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if(rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }

        
        

        
        //limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            //limit velocity

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;

                if (!grounded)
                {
                    limitedVel *= airMultiplayer;
                }
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

    }

    private void Jump()
    {
        exitingSlope = true;
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out SlopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, SlopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, SlopeHit.normal).normalized;
    }
    private void UpdateCrouchScale()
    {
        // Adjust character's scale based on crouching state.
        if (isCrouching)
        {
            transform.localScale = new Vector3(transform.localScale.x, crounchYScale, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, startYscale, transform.localScale.z);
        }
    }
}
