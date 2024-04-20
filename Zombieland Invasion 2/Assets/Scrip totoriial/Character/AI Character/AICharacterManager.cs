using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICharacterManager : CharacterManager
{
    [Header("Character Name")]
    public string characterName = "";

    [HideInInspector] public AICharacterNetworkManager aICharacterNetworkManager;
    [HideInInspector] public AICharacterCombatManager aICharacterCombatManager;
    [HideInInspector] public AICharacterLocomotionManager aICharacterLocomotionManager;

    [Header("Navmesh Agent")]
    public NavMeshAgent navMeshAgent;

    [Header("Current State")]
    [SerializeField] protected AIState currentState;

    [Header("States")]
    public IdleState idle;
    public PursueTargetState pursueTarget;
    public CombatStanceState combatStance;
    public AttackState attack;

    protected override void Awake()
    {
        base.Awake();

        aICharacterNetworkManager = GetComponent<AICharacterNetworkManager>();
        aICharacterCombatManager = GetComponent<AICharacterCombatManager>();
        aICharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();

        navMeshAgent = GetComponentInChildren<NavMeshAgent>();

        // use a copy of the scriptable object so the originals are not modified
        
        
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            idle = Instantiate(idle);
            pursueTarget = Instantiate(pursueTarget);

            currentState = idle;
        }
    }

    protected override void Update()
    {
        base.Update();

        aICharacterCombatManager.HandleActionRecovery(this);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(IsOwner)
        {
            ProcessStateMachine();
        }
        
    }
    private void ProcessStateMachine()
    {
        AIState nextState = currentState?.Tick(this);

        if (currentState != null)
        {
            currentState = nextState;
        }

        // the position/rotation should be reset after the state machine has processed it's tick
        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;

        if(aICharacterCombatManager.currentTarget != null)
        {
            aICharacterCombatManager.targetsDirection = aICharacterCombatManager.currentTarget.transform.position - transform.position;
            aICharacterCombatManager.viewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(transform, aICharacterCombatManager.targetsDirection);
            aICharacterCombatManager.distanceFromTarget = Vector3.Distance(transform.position, aICharacterCombatManager.currentTarget.transform.position);
        }

        if (navMeshAgent.enabled)
        {
            Vector3 agentDestination = navMeshAgent.destination;
            float remainingDistance = Vector3.Distance(agentDestination, transform.position);

            if (remainingDistance > navMeshAgent.stoppingDistance)
            {
                aICharacterNetworkManager.isMoving.Value = true;
            }
            else
            {
                aICharacterNetworkManager.isMoving.Value = false;
            }
        }
        else
        {
            aICharacterNetworkManager.isMoving.Value = false;
        }
    }
}
