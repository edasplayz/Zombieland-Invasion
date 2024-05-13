using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterLocomotionManager : CharacterLocomationManager
{
   

    public void RotateTowardsAgent(AICharacterManager aICharacter)
    {
        if(aICharacter.aICharacterNetworkManager.isMoving.Value)
        {
            aICharacter.transform.rotation = aICharacter.navMeshAgent.transform.rotation;
        }
    }
}
