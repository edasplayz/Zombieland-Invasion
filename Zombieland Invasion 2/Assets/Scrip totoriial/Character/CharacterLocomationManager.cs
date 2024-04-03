using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomationManager : MonoBehaviour
{
    CharacterManager character;
    [Header("Ground Check and Jumping")]
    [SerializeField] float gavityForce = -5.55f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckSphereRadius = 1;
    [SerializeField] protected Vector3 yVelocity; // the force which our character is pulled up or down( jumping or falling)
    [SerializeField] protected float groundedYVelocity = -20;// the force which our character is sticking to the ground whilst they grounded 
    [SerializeField] protected float fallStartYVelocity = -5; // the force at which our character beggins to fall when they become ungrounded (rises ads they fall longer)
    protected bool fallingVelocityHasBeenSet = false;
    protected float inAirTimer = 0;
    
    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Update()
    {
        HandleGroundCheck();

        if (character.isGrounded)
        {
            // if we arre not attemting to jump or move upward
            if(yVelocity.y < 0)
            {
                inAirTimer = 0;
                fallingVelocityHasBeenSet = false;
                yVelocity.y = groundedYVelocity;
            }
        }
        else
        {
            // if we are not jumping ant our falling velocity has not ben set
            if(!character.isJumping && !fallingVelocityHasBeenSet)
            {
                fallingVelocityHasBeenSet = true;
                yVelocity.y = fallStartYVelocity;
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            character.animator.SetFloat("InAirTimer", inAirTimer);

            yVelocity.y += gavityForce * Time.deltaTime;

            
        }
        // there should alwais be some force applied to the y velocity
        character.characterController.Move(yVelocity * Time.deltaTime);
    }

    protected void HandleGroundCheck()
    {
        character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
    }

    // draw our ground check sphere in scene view 
    protected void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
    }
}
