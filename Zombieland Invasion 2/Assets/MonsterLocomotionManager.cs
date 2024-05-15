using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Invector.vCharacterController.vThirdPersonMotor;
using static UnityEngine.GraphicsBuffer;

public class MonsterLocomotionManager : CharacterLocomationManager
{
    [SerializeField] protected float rotationSpeed; 
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected Transform target;
    public void RotateTowardsAgent(AICharacterManager aICharacter)
    {
        if (aICharacter.aICharacterNetworkManager.isMoving.Value)
        {
            aICharacter.transform.rotation = aICharacter.navMeshAgent.transform.rotation;
        }
    }

    protected override void Update()
    {
        base.Update(); // Call the base class Update for ground check and gravity

        if (target != null && canMove)
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        if (character.characterController == null)
        {
            Debug.LogError("CharacterController not found on CharacterManager. Movement may not work as expected.");
            return;
        }

        // Calculate direction towards target
        Vector3 targetDirection = target.position - transform.position;
        targetDirection.y = 0f; // Ignore vertical movement

        // Normalize direction for unit vector
        targetDirection.Normalize();

        // Smooth rotation towards target (optional)
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), rotationSpeed * Time.deltaTime);

        // Move character towards target (adjust speed if needed)
        character.characterController.Move(targetDirection * movementSpeed * Time.deltaTime);
    }
}
