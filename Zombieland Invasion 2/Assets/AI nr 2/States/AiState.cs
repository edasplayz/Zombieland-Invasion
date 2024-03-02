using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AiStateID
{
    ChasePlayer
}

public interface AiState 
{
    AiStateID GetId();
    void Enter(AiAgent agent);
    void Update(AiAgent agent);
    void Exit(AiAgent agent);
}
