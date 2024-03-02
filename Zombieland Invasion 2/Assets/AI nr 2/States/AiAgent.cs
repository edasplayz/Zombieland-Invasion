using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    
    public AiStateMachine stateMachine;
    public AiStateID initiolState;
    public NavMeshAgent navMeshAgent;
    
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterStates(new AiChasePlayer());
        stateMachine.ChangeState(initiolState);
    }
    private void Update()
    {
        stateMachine.Update();
    }
}
