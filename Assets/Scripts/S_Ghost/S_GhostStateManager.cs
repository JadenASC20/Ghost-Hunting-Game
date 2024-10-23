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

    // Randomness variables
    private float randomOffset; // Offset to apply to waypoints
    private System.Random random; // Random object for unique randomness

    // Current active state of the ghost
    public S_GhostBaseState currentState;

    // References to the ghost's states
    [HideInInspector] public S_GhostPatrolState PatrolState;
    [HideInInspector] public S_GhostAttackState AttackState;
    [HideInInspector] public S_GhostFleeState FleeState;
    [HideInInspector] public S_GhostStunState StunState;
    [HideInInspector] public S_GhostVacuumedState VacuumedState;

    // Reference to the GhostManager
    public GhostManager ghostManager; 

    void Start()
    {
        PatrolState = new S_GhostPatrolState();
        AttackState = new S_GhostAttackState();
        FleeState = new S_GhostFleeState();
        StunState = new S_GhostStunState();
        VacuumedState = new S_GhostVacuumedState();

        agent = GetComponent<NavMeshAgent>();

        // Initialize random seed
        random = new System.Random(GetInstanceID());
        randomOffset = (float)(random.NextDouble() * 2 - 1); // Random value between -1 and 1

        // Set the GhostManager reference for states
        VacuumedState.SetGhostManager(ghostManager);

        currentState = FleeState;
        currentState.EnterState(this);
    }

    public float GetRandomOffset()
    {
        return randomOffset;
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
        Debug.Log($"{gameObject.name} has been killed.");
        Destroy(gameObject);
    }
}
