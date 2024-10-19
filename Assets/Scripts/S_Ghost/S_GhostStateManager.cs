using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class S_GhostStateManager : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject Player;
    public List<Transform> waypoints;
    public Transform chosenWaypoint;
    public float stunPowerMultiplier = 1f;
    
    // current active state of the ghost
    S_GhostBaseState currentState;

    // references to the ghosts state
    [HideInInspector] public S_GhostPatrolState PatrolState;
    [HideInInspector] public S_GhostAttackState AttackState;
    [HideInInspector] public S_GhostFleeState FleeState;
    [HideInInspector] public S_GhostStunState StunState;
    [HideInInspector] public S_GhostVacuumedState VacuumedState;

    void Start()
    {
        PatrolState = new S_GhostPatrolState();
        AttackState = new S_GhostAttackState();
        FleeState = new S_GhostFleeState();
        StunState = new S_GhostStunState();
        VacuumedState = new S_GhostVacuumedState();

        agent = GetComponent<NavMeshAgent>();

        currentState = FleeState;
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    void OnTriggerEnter(Collider collider)
    {
        currentState.OnSpiritTriggerEnter(this, collider);
    }

    public void SwitchState(S_GhostBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public void PlaySound()
    {
        // play sound logic
    }
}
