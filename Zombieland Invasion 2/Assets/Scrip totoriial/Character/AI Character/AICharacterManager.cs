using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterManager : CharacterManager
{
    public AICharacterCombatManager aICharacterCombatManager;

    [Header("Current State")]
    [SerializeField] AIState currentState;

    protected override void Awake()
    {
        base.Awake(); 

        aICharacterCombatManager = GetComponent<AICharacterCombatManager>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        ProcessStateMachine();
    }
    private void ProcessStateMachine()
    {
        AIState nextState = currentState?.Tick(this);

        if (currentState != null)
        {
            currentState = nextState;
        }
    }
}
