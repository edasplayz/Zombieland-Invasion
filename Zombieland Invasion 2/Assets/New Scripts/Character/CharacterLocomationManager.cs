using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomationManager : MonoBehaviour
{
    public CharacterManager character;

    [Header("Ground Check and Jumping")]
    [SerializeField] protected float gravityForce = -5.55f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckSphereRadius = 1;
    [SerializeField] protected Vector3 yVelocity; // the force which our character is pulled up or down( jumping or falling)
    [SerializeField] protected float groundedYVelocity = -20;// the force which our character is sticking to the ground whilst they grounded 
    [SerializeField] protected float fallStartYVelocity = -5; // the force at which our character beggins to fall when they become ungrounded (rises ads they fall longer)
    protected bool fallingVelocityHasBeenSet = false;
    protected float inAirTimer = 0;
    [SerializeField] bool freeze = false;

    [Header("Flags")]
    public bool isRolling = false;
    public bool canRotate = true;
    public bool canMove = true;
    public bool isGrounded = true;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Update()
    {
        if (!freeze)
        {


            HandleGroundCheck();

            if (character.characterLocomationManager.isGrounded)
            {
                // if we arre not attemting to jump or move upward
                if (yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                // if we are not jumping ant our falling velocity has not ben set
                if (!character.characterNetworkManager.isJumping.Value && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }

                inAirTimer = inAirTimer + Time.deltaTime;
                character.animator.SetFloat("InAirTimer", inAirTimer);

                yVelocity.y += gravityForce * Time.deltaTime;


            }

            // there should alwais be some force applied to the y velocity
            character.characterController.Move(yVelocity * Time.deltaTime);
        }
    }

    protected void HandleGroundCheck()
    {
        character.characterLocomationManager.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
    }

    public void EnableCanRotate()
    {
        canRotate = true;
    }
    public void DisableCanRotate()
    {
        canRotate = false;
    }

    public void EnableFreeze()
    {
        freeze = true;

        Invoke("DisableFreeze", 1f);
    }

    public void DisableFreeze()
    {
        freeze = false;
        Debug.Log("Unfreez");
    }

    // draw our ground check sphere in scene view 
    protected void OnDrawGizmosSelected()
    {
        if (character != null && character.transform != null)
        {
            Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
        }
        else
        {
            //Debug.LogWarning("Character or character transform is null. Gizmos will not be drawn.");
        }
    }

   
}
