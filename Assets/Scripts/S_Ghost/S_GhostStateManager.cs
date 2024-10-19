//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class S_GhostStateManager : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject Player;
    public List<Transform> waypoints;
    public Transform chosenWaypoint;
    public float stunPowerMultiplier = 1f;

    // Current active state of the ghost
    public S_GhostBaseState currentState;

    // References to the ghost's states
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
        // Play sound logic
    }

    public void KillGhost()
    {
        // Handle ghost destruction or death here
        Debug.Log($"{gameObject.name} has been killed.");
        // Optionally play death animation or sound
        Destroy(gameObject); // Example: Destroy the ghost
    }
}
