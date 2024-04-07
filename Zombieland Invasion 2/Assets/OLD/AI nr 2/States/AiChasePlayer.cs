using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiChasePlayer : AiState
{
    //public Transform playerTransform;
    public AiLocomotion position;
    
    public void Enter(AiAgent agent)
    {
        
    }

    public void Exit(AiAgent agent)
    {
        
    }

    public AiStateID GetId()
    {
        return AiStateID.ChasePlayer;
    }

    public void Update(AiAgent agent )
    {
       //agent.navMeshAgent.destination = position.playerTransform.position;
    }
}
