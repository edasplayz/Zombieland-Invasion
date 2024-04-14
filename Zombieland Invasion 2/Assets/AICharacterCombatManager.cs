using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterCombatManager : CharacterCombatManager
{

    [Header("Detection")]
    [SerializeField] float detectionRadius = 15;
    [SerializeField] float minimumDetectionAngle = -35;
    [SerializeField] float maximumDetectionAngle = 35;
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
                float viewableAngle = Vector3.Angle(targetDirection, aICharacter.transform.forward);

                if(viewableAngle > minimumDetectionAngle && viewableAngle < maximumDetectionAngle)
                {
                    // lastly we check for enviromental blocks
                    if(Physics.Linecast(
                        aICharacter.characterCombatManager.lockOnTransform.position, 
                        targetCharacter.characterCombatManager.lockOnTransform.position, 
                        WorldUtilityManager.Instance.GetEnviroLayers()))
                    {
                        Debug.DrawLine(aICharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position);
                        Debug.Log("Blocked");
                    }
                    else
                    {
                        aICharacter.characterCombatManager.SetTarget(targetCharacter);
                    }
                }
            }
        }
    }
}
