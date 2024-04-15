using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/States/Idle")]
public class IdleState : AIState
{
    public override AIState Tick(AICharacterManager aICharacter)
    {
        if(aICharacter.characterCombatManager.currentTarget != null)
        {
            //return the pursue target state(change the state to the pursue target state)
            return SwitchState(aICharacter, aICharacter.pursueTarget);
            
        }
        else
        {
            // return this state to continually search for a target (keep the state here until a target is found)
            aICharacter.aICharacterCombatManager.FindATargetViaLineOfSight(aICharacter);
            return this;
        }

        
    }

    
}
