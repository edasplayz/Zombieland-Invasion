using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/States/Attack")]
public class AttackState : AIState
{
    [Header("Current Attack")]
    [HideInInspector] public AICharacterAttackAction currentAttack;
    [HideInInspector] public bool willPerformCombo = false;

    [Header("State Flags")]
    protected bool hasPerformedAttack = false;
    protected bool hasPerformedCombo = false;

    [Header("Pivot After Attack")]
    [SerializeField] protected bool pivotAfterAttack = false;

    public override AIState Tick(AICharacterManager aICharacter)
    {
        if(aICharacter.aICharacterCombatManager.currentTarget == null)
        {
            return SwitchState(aICharacter, aICharacter.idle);
        }

        if(aICharacter.aICharacterCombatManager.currentTarget.isDead.Value)
        {
            return SwitchState(aICharacter, aICharacter.idle);
        }

        // rotate towards the target whilst attakink
        aICharacter.aICharacterCombatManager.RotateTowardsTargetWhilstAttacking(aICharacter);


        // set movement values to 0
        aICharacter.characterAnimatorManager.UpdateAnimatorMovementParameters(0, 0, false);

        // perform a combo
        if(willPerformCombo && !hasPerformedCombo)
        {
            if(currentAttack.comboAction !=  null)
            {
                // if can combo
               // hasPerformedCombo = true;
               // currentAttack.comboAction.AttemptToPerformAction(aICharacter);
            }
        }

        if (aICharacter.isPreformingAction)
        {
            return this;
        }

        if (!hasPerformedAttack)
        {
            // if we arre still recovering from an action wait before performing another
            if(aICharacter.aICharacterCombatManager.actionRecoveryTimer > 0)
            {
                return this;
            }
            
            PerformAttack(aICharacter);


            //return to the top so if we have combo we process that when we are able
            return this;
            
        }

        if (pivotAfterAttack)
        {
            aICharacter.aICharacterCombatManager.TurnTowardsTarget(aICharacter);
        }

        return SwitchState(aICharacter, aICharacter.combatStance);
    }

    protected void PerformAttack(AICharacterManager aICharacter)
    {
        hasPerformedAttack = true;
        currentAttack.AttemptToPerformAction(aICharacter);
        aICharacter.aICharacterCombatManager.actionRecoveryTimer = currentAttack.actionRecoveeryTime;
    }

    protected override void ResetStateFlags(AICharacterManager aICharacter)
    {
        base.ResetStateFlags(aICharacter);

        hasPerformedAttack = false;
        hasPerformedCombo = false;
    }

}
