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
    private float randomOffset;
    private System.Random random;

    public S_GhostBaseState currentState;

    [HideInInspector] public S_GhostPatrolState PatrolState;
    [HideInInspector] public S_GhostAttackState AttackState;
    [HideInInspector] public S_GhostFleeState FleeState;
    [HideInInspector] public S_GhostStunState StunState;
    [HideInInspector] public S_GhostVacuumedState VacuumedState;

    // Reference to the GhostHealthManager
    public S_GhostHealthManager ghostHealthManager;

    void Start()
    {
        // Initialize states
        PatrolState = new S_GhostPatrolState();
        AttackState = new S_GhostAttackState();
        FleeState = new S_GhostFleeState();
        StunState = new S_GhostStunState();
        VacuumedState = new S_GhostVacuumedState();

        // Get the NavMeshAgent and the health manager component
        agent = GetComponent<NavMeshAgent>();
        ghostHealthManager = GetComponent<S_GhostHealthManager>();

        // Set the initial state
        currentState = FleeState;
        currentState.EnterState(this);

        // Random offset for ghost behavior
        random = new System.Random(GetInstanceID());
        randomOffset = (float)(random.NextDouble() * 2 - 1); // Random value between -1 and 1
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


