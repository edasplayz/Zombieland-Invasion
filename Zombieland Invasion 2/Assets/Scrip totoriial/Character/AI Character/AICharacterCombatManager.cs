using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterCombatManager : CharacterCombatManager
{
    [Header("Target Information")]
    public float distanceFromTarget;
    public float viewableAngle;
    public Vector3 targetsDirection;


    [Header("Detection")]
    [SerializeField] float detectionRadius = 15;
    public float minimumFOV = -35;
    public float maximumFOV = 35;
    public void FindATargetViaLineOfSight(AICharacterManager aICharacter)
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
                // if a potensial target is found it has to be infront of us
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
                        PivotTowardsTarget(aICharacter);
                    }
                }
            }
        }
    }

    public void PivotTowardsTarget(AICharacterManager aICharacter)
    {
        // play a pivot animation depending on viewable angle of target
        if (aICharacter.isPreformingAction)
        {
            return;
        }

        if(viewableAngle >= 20 && viewableAngle <= 60)
        {
            aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_R_45", true);
        }
        else if (viewableAngle <= -20 && viewableAngle >= 60)
        {
            aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_L_45", true);
        }
        else if (viewableAngle >= 61 && viewableAngle <= 110)
        {
            aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_R_90", true);
        }
        else if (viewableAngle <= -61 && viewableAngle >= -110)
        {
            aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_L_90", true);
        }
        else if (viewableAngle >= 110 && viewableAngle <= 145)
        {
            aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_R_135", true);
        }
        else if (viewableAngle <= -110 && viewableAngle >= -145)
        {
            aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_L_135", true);
        }
        else if (viewableAngle >= 146 && viewableAngle <= 180)
        {
            aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_R_180", true);
        }
        else if (viewableAngle <= -146 && viewableAngle >= -180)
        {
            aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_L_180", true);
        }

    }
}
