using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    int vertical;
    int horizontal;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();

        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }
    public void UpdateAnimatorMovementParameters(float horizontalValues, float verticalValues, bool isSprinting)
    {

        float horizontalAmount = horizontalValues;
        float verticalAmount = verticalValues;


        if(isSprinting )
        {
            verticalAmount = 2;
        }

        character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
        character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
    }

    public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false)
    {
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        // can be used to stop character form attemting new action
        // form exaple if you get damaged and begin performing a damage animation
        // this flag will turn true if you are stunned 
        // we can then check for this before attempting new actions
        character.isPreformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;

        // tell the server/host we player an animation, and to play that animation for everyone else present 
        character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }

    public virtual void PlayTargetAttackActionAnimation(AttackType attackType, string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false)
    {

        // keep track of last attack performed (for combos)
        // keep tharck of current attack type (light, heavy, ect)
        // update animation set to current weapon animations
        // decide if our attack can be parried
        // tell the network our (isattacking) floag is active (for counter damage ect)
        character.characterCombatManager.currentAttackType = attackType;
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        character.isPreformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;

        // tell the server/host we player an animation, and to play that animation for everyone else present 
        character.characterNetworkManager.NotifyTheServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }
}
