using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;


public class MonsterLocomotionManager : CharacterLocomationManager
{
    
    public void RotateTowardsAgent(AICharacterManager aICharacter)
    {
        if (aICharacter.aICharacterNetworkManager.isMoving.Value)
        {
            aICharacter.transform.rotation = aICharacter.navMeshAgent.transform.rotation;
        }
    }

    
}
