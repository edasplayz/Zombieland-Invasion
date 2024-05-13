using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ResetActionFlag : StateMachineBehaviour
{
    CharacterManager character;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(character == null)
        {
            character =animator.GetComponent<CharacterManager>();
        }

        // this is called when an action ends, and the state returns to empty
        character.isPreformingAction = false;
        character.characterAnimatorManager.applyRootMotion = false;
        character.characterLocomationManager.canMove = true;
        character.characterLocomationManager.canRotate = true;
        character.characterLocomationManager.isRolling = false;
        character.characterCombatManager.DisableCnDoCombos();
        character.characterCombatManager.DisableCanDoRollinAttack();
        character.characterCombatManager.DisableCanDoBackStepAttack();

        if (character.IsOwner)
        {
            character.characterNetworkManager.isJumping.Value = false;
            character.characterNetworkManager.isInvulnerable.Value = false;
            character.characterNetworkManager.isAttacking.Value = false;
        }
        
        
        //character.animator.applyRootMotion = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
