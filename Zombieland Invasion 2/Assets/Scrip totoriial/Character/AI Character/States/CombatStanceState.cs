using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "A.I/States/Combat Stance")]
public class CombatStanceState : AIState
{
    // what in this script
    // 1 select an attack for the attack state depending on distance and angle of target in relation to character
    // process any combat logic here whilst waiting to attack (blocking srafing dodging)
    // if target moves out of combat range switch to pursue target 
    // if target is no longer present switch to idle state

    [Header("Attacks")]
    public List<AICharacterAttackAction> aICharacterAttacks; // a list of all possible attacks this character can do 
    protected List<AICharacterAttackAction> potensialAttacks; // all attacks possible in this situasion (based on angle distance ect)
    private AICharacterAttackAction choosenAttack;
    private AICharacterAttackAction previousAttack;
    protected bool hasAttack = false;

    [Header("Combo")]
    [SerializeField] protected bool canPerformCombo = false; // if the character can performe a combo attack after the initial attack
    [SerializeField] protected int chanceToPerformCombo = 25; // the chance (in percent) of the character to perform a combo on the next attack
    protected bool hasRolledForComboChance = false; // if we have already rolled for the chance during this state

    [Header("Engagement Distance")]
    [SerializeField] protected float maximumEngagementDistance = 5; // the distance we have to be away from the target before we enter the pursue target state

    public override AIState Tick(AICharacterManager aICharacter)
    {
        if(aICharacter.isPreformingAction)
        {
            return this;
        }
        if(!aICharacter.navMeshAgent.enabled)
        {
            aICharacter.navMeshAgent.enabled = true;
        }

        // if you want the ai character to face and turn towards its target when its outside its fov include this
        if (!aICharacter.aICharacterNetworkManager.isMoving.Value)
        {
            if(aICharacter.aICharacterCombatManager.viewableAngle < -30 || aICharacter.aICharacterCombatManager.viewableAngle > 30)
            {
                aICharacter.aICharacterCombatManager.PivotTowardsTarget(aICharacter);
            }
        }

        // rotate to face our target 

        // if our target is no longer present switch back to idle
        if(aICharacter.aICharacterCombatManager.currentTarget == null)
        {
            return SwitchState(aICharacter, aICharacter.idle);
        }

        // if we not have an attack get one
        if(!hasAttack)
        {
            GetNewAttack(aICharacter);
        }
        else
        {
            // check recovery timer
            // pass attack to attack state
            // roll for combo chance
            // switch state
        }
        // if we are outside of the combat engagement distance switch to pursue target state
        if(aICharacter.aICharacterCombatManager.distanceFromTarget > maximumEngagementDistance)
        {
            return SwitchState(aICharacter, aICharacter.pursueTarget);
        }

        NavMeshPath path = new NavMeshPath();
        aICharacter.navMeshAgent.CalculatePath(aICharacter.aICharacterCombatManager.currentTarget.transform.position, path);
        aICharacter.navMeshAgent.SetPath(path);

        return this;


    }

    protected virtual void GetNewAttack(AICharacterManager aICharacter)
    {
        potensialAttacks = new List<AICharacterAttackAction>();

        foreach (var potentialAttack in potensialAttacks)
        {
            // if we are to close for this attack check the next
            if(potentialAttack.minimumAttackDistance > aICharacter.aICharacterCombatManager.distanceFromTarget)
            {
                continue;
            }
            // if we are to far for this attack check nexr
            if (potentialAttack.maximumAttackDistance < aICharacter.aICharacterCombatManager.distanceFromTarget)
            {
                continue;
            }
            // if the attack is outside minimum field of view for this attack check the next
            if (potentialAttack.minimumAttackAngle > aICharacter.aICharacterCombatManager.viewableAngle)
            {
                continue;
            }
            // if the target is ousite maximum field of view for this attack check the next
            if (potentialAttack.maximumAttackAngle < aICharacter.aICharacterCombatManager.viewableAngle)
            {
                continue;
            }

            potensialAttacks.Add(potentialAttack);
        }

        if(potensialAttacks.Count <= 0)
        {
            Debug.Log("forgot add attack as scriptable objects");
            return; 
        }

        var totalWeight = 0;

        foreach(var attack in potensialAttacks)
        {
            totalWeight += attack.attackWeight;
        }

        var randomWeightValue = Random.Range(1, totalWeight + 1);
        var processedWeight = 0;

        foreach( var attack in potensialAttacks)
        {
            processedWeight += attack.attackWeight;

            if(randomWeightValue <= processedWeight)
            {
                // this is our attack
                choosenAttack = attack;
                previousAttack = choosenAttack;
                hasAttack = true;
            } 
        }
        
       
    }

    protected virtual bool RollForOutcomeChance(int outcomeChance)
    {
        bool outcomeWillBePerformed = false;

        int randomPercentage = Random.Range(0, 100);

        if(randomPercentage < outcomeChance)
        {
            outcomeWillBePerformed = true;
        }
        return outcomeWillBePerformed;
    }

    protected override void ResetStateFlags(AICharacterManager aICharacter)
    {
        base.ResetStateFlags(aICharacter);
        hasAttack = false;
        hasRolledForComboChance = false;

        
    }

}
