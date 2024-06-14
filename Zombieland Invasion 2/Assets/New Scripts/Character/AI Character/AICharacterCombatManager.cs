using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterCombatManager : CharacterCombatManager
{
    protected AICharacterManager aiCharacter;

    [Header("Action Recovery")]
    public float actionRecoveryTimer = 0;

    [Header("Pivot")]
    public bool enablePivot = true;

    [Header("Target Information")]
    public float distanceFromTarget;
    public float viewableAngle;
    public Vector3 targetsDirection;


    [Header("Detection")]
    [SerializeField] float detectionRadius = 15;
    public float minimumFOV = -35;
    public float maximumFOV = 35;

    [Header("Attack Rotation Speed")]
    public float attackRotationSpeed = 25;

    protected override void Awake()
    {
        base.Awake();

        aiCharacter = GetComponent<AICharacterManager>();

        lockOnTransform = GetComponentInChildren<LockOnTransform>().transform;


    }
    public void FindATargetInItsLineOfSight(AICharacterManager aICharacter)
    {
        if(currentTarget != null)
        {
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(aICharacter.transform.position, detectionRadius, WorldUtilityManager.Instance.GetCharacterLayers());

        for(int i = 0; i < colliders.Length; i++)
        {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

            if(targetCharacter == null)
            {
                continue;
            }
            if(targetCharacter == aICharacter)
            {
                continue;
            }
            if(targetCharacter.isDead.Value)
            {
                continue;
            }

            // can i attack this character if so make them my target
            if (WorldUtilityManager.Instance.CanIDamageThisTarget(aICharacter.characterGroup, targetCharacter.characterGroup))
            {
                // if a potensial target is found it has to be in front of us
                Vector3 targetDirection = targetCharacter.transform.position - aICharacter.transform.position;
                float angleOfPotentialTarget = Vector3.Angle(targetDirection, aICharacter.transform.forward);

                if(angleOfPotentialTarget > minimumFOV && angleOfPotentialTarget < maximumFOV)
                {
                    // lastly we check for enviromental blocks
                    if(Physics.Linecast(
                        aICharacter.characterCombatManager.lockOnTransform.position, 
                        targetCharacter.characterCombatManager.lockOnTransform.position, 
                        WorldUtilityManager.Instance.GetEnviroLayers()))
                    {
                        Debug.DrawLine(aICharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position);
                        
                    }
                    else
                    {
                        targetDirection = targetCharacter.transform.position - transform.position;
                        viewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(transform, targetsDirection);
                        aICharacter.characterCombatManager.SetTarget(targetCharacter);

                        if(enablePivot)
                        {
                            TurnTowardsTarget(aICharacter);
                        }
                        
                    }
                }
            }
        }
    }

    public virtual void TurnTowardsTarget(AICharacterManager aICharacter)
    {
        // play a turn animation depending on viewable angle of target the commented area is if later i want that the turns would be more accurate
        if (aICharacter.isPreformingAction)
        {
            return;
        }

        /*if(viewableAngle >= 20 && viewableAngle <= 60)
        {
            aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_R_45", true);
        }
        else if (viewableAngle <= -20 && viewableAngle >= 60)
        {
            aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_L_45", true);
        }
        */if (viewableAngle >= 61 && viewableAngle <= 110)
        {
            aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_R_90", true);
        }
        else if (viewableAngle <= -61 && viewableAngle >= -110)
        {
            aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_L_90", true);
        }
       /* else if (viewableAngle >= 110 && viewableAngle <= 145)
        {
            aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_R_135", true);
        }
        else if (viewableAngle <= -110 && viewableAngle >= -145)
        {
            aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_L_135", true);
        }
        */else if (viewableAngle >= 146 && viewableAngle <= 180)
        {
            aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_R_180", true);
        }
        else if (viewableAngle <= -146 && viewableAngle >= -180)
        {
            aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_L_180", true);
        }

    }

    public void RotateTowardsAgent(AICharacterManager aICharacter)
    {
        if(aICharacter.aICharacterNetworkManager.isMoving.Value)
        {
            aICharacter.transform.rotation = aICharacter.navMeshAgent.transform.rotation;
        }
    }

    public void RotateTowardsTargetWhilstAttacking(AICharacterManager aICharacter)
    {
        if(currentTarget == null)
        {
            return;
        }

        if (!aICharacter.characterLocomationManager.canRotate)
        {
            return;
        }

        if(!aICharacter.isPreformingAction)
        {
            return;
        }

        Vector3 targetDirection = currentTarget.transform.position - aICharacter.transform.position;
        targetDirection.y = 0;
        targetDirection.Normalize();

        if(targetDirection == Vector3.zero)
        {
            targetDirection = aICharacter.transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        aICharacter.transform.rotation = Quaternion.Slerp(aICharacter.transform.rotation, targetRotation, attackRotationSpeed * Time.deltaTime);
    }
    public void HandleActionRecovery(AICharacterManager aICharacter)
    {
        if(actionRecoveryTimer > 0)
        {
            if(!aICharacter.isPreformingAction)
            {
                actionRecoveryTimer -= Time.deltaTime;
            }
        }
    }
}
