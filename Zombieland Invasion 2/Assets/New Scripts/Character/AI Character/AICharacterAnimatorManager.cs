using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterAnimatorManager : CharacterAnimatorManager
{
    AICharacterManager aICharacter;

    protected override void Awake()
    {
        base.Awake();

        aICharacter = GetComponent<AICharacterManager>();
    }

    private void OnAnimatorMove()
    {

        //host
        if(aICharacter.IsOwner)
        {
            if(!aICharacter.characterLocomationManager.isGrounded)
            {
                return;
            }

            Vector3 velocity = aICharacter.animator.deltaPosition;

            aICharacter.characterController.Move(velocity);
            aICharacter.transform.rotation *= aICharacter.animator.deltaRotation;
        }
        //client
        else
        {
            if (!aICharacter.characterLocomationManager.isGrounded)
            {
                return;
            }

            Vector3 velocity = aICharacter.animator.deltaPosition;

            aICharacter.characterController.Move(velocity);
            aICharacter.transform.position = Vector3.SmoothDamp(transform.position, 
                aICharacter.characterNetworkManager.networkPosition.Value, 
                ref aICharacter.characterNetworkManager.networkPositionVelocity,
                aICharacter.characterNetworkManager.networkPositionSmoothTime);
            aICharacter.transform.rotation *= aICharacter.animator.deltaRotation;
        }
    }
}
