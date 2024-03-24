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
}
