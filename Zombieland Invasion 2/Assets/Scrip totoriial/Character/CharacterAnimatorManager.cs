using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    float vertical;
    float horizontal;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    public void UpdateAnimatorMovementParameters(float horizontalValues, float verticalValues)
    {
        // option 1
        character.animator.SetFloat("Horizontal", horizontalValues, 0.1f, Time.deltaTime);
        character.animator.SetFloat("Vertical", verticalValues, 0.1f, Time.deltaTime);
    }

    public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion = true)
    {
        character.animator.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        // can be used to stop character form attemting new action
        // form exaple if you get damaged and begin performing a damage animation
        // this flag will turn true if you are stunned 
        // we can then check for this before attempting new actions
        character.isPreformingAction = isPerformingAction;


    }
}