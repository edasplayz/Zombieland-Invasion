using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "A.I/States/Pursue Target")]
public class PursueTargetState : AIState
{
    
    public override AIState Tick(AICharacterManager aICharacter)
    {
        // check if we are performing an action (if so do nothing until action is complete)
        if (aICharacter.isPreformingAction)
        {
            return this;
        }

        //check if our atrget is null if we do have a target return to idle 
        if(aICharacter.aICharacterCombatManager.currentTarget == null)
        {
            return SwitchState(aICharacter, aICharacter.idle);
        }

        // make sure our navmesh agent is active if its not enable it
        if(!aICharacter.navMeshAgent.enabled)
        {
            aICharacter.navMeshAgent.enabled = true;
        }
        // if our target goes outside of the character f.o.v pivot to face them 
        // this is the part that makes character turn towards player
        if(aICharacter.aICharacterCombatManager.enablePivot)
        {
            if (aICharacter.aICharacterCombatManager.viewableAngle < aICharacter.aICharacterCombatManager.minimumFOV
            || aICharacter.aICharacterCombatManager.viewableAngle > aICharacter.aICharacterCombatManager.maximumFOV)
            {
                aICharacter.aICharacterCombatManager.TurnTowardsTarget(aICharacter);
            }
        }
        aICharacter.aICharacterLocomotionManager.RotateTowardsAgent(aICharacter);

        // if we are within combat range of a target switch state to combat stance state

        if (aICharacter.aICharacterCombatManager.distanceFromTarget <= aICharacter.navMeshAgent.stoppingDistance)
        {
            return SwitchState(aICharacter, aICharacter.combatStance);
        }

        // pursue the target

        NavMeshPath path = new NavMeshPath();
        aICharacter.navMeshAgent.CalculatePath(aICharacter.aICharacterCombatManager.currentTarget.transform.position, path);
        aICharacter.navMeshAgent.SetPath(path);

        return this;
    }
}
