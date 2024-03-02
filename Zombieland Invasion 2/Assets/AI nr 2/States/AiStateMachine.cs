using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiStateMachine
{
    public AiState[] states;
    public AiAgent agent;
    public AiStateID currentState;

    public AiStateMachine(AiAgent agent)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(AiStateID)).Length;
        states = new AiState[numStates];
    }

    public void RegisterStates(AiState state)
    {
        int index = (int)state.GetId();
        states[index] = state;
    }

    public AiState GetState(AiStateID statesId)
    {
        int index = (int)statesId;
        return states[index];
    }
    public void Update()
    {
        GetState(currentState)?.Update(agent);
    }

    public void ChangeState(AiStateID newState)
    {
        GetState(currentState)?.Exit(agent);
        currentState = newState;
        GetState(currentState)?.Enter(agent);
    }
}
