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
        // this is the part that makes character rotate randomly on zombie its ok bu on smarter enemy is bad 
        /*if(aICharacter.aICharacterCombatManager.viewableAngle < aICharacter.aICharacterCombatManager.minimumFOV 
            || aICharacter.aICharacterCombatManager.viewableAngle > aICharacter.aICharacterCombatManager.maximumFOV)
        {
            aICharacter.aICharacterCombatManager.PivotTowardsTarget(aICharacter);
        }*/

        aICharacter.aICharacterLocomotionManager.RotateTowardsAgent(aICharacter);

        // if we are within combat range of a target switch state to combat stance state

        // option 1 
        /*if(aICharacter.aICharacterCombatManager.distanceFromTarget <= aICharacter.combatStance.maximumEngagementDistance)
        {
            return SwitchState(aICharacter, aICharacter.combatStance);
        }*/

        // option 2
        if (aICharacter.aICharacterCombatManager.distanceFromTarget <= aICharacter.navMeshAgent.stoppingDistance)
        {
            return SwitchState(aICharacter, aICharacter.combatStance);
        }
        


        // if the target is not reachable and they are far away return home

        // pursue the target

        //option 1 
        //aICharacter.navMeshAgent.SetDestination(aICharacter.aICharacterCombatManager.currentTarget.transform.position);

        //option 2
        NavMeshPath path = new NavMeshPath();
        aICharacter.navMeshAgent.CalculatePath(aICharacter.aICharacterCombatManager.currentTarget.transform.position, path);
        aICharacter.navMeshAgent.SetPath(path);

        return this;
    }
}
