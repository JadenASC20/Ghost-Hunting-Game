using UnityEngine;

public class S_GhostPatrolState : S_GhostBaseState
{
    private float patrolSpeed = 2f; // Speed during patrol
    private float reachedThreshold = 0.1f; // Distance to consider the waypoint reached
    private Transform targetWaypoint; // Current target waypoint

    public override void EnterState(S_GhostStateManager sGhost)
    {
        Debug.Log("Ghost is Patrolling...");
        ChooseRandomWaypoint(sGhost); // Choose a random waypoint when entering patrol state
    }

    public override void UpdateState(S_GhostStateManager sGhost)
    {
        if (targetWaypoint == null) return; // No target waypoint

        // Move towards the current target waypoint
        sGhost.transform.position = Vector3.MoveTowards(sGhost.transform.position, targetWaypoint.position, patrolSpeed * Time.deltaTime);

        // Check if the ghost has reached the waypoint
        if (Vector3.Distance(sGhost.transform.position, targetWaypoint.position) < reachedThreshold)
        {
            ChooseRandomWaypoint(sGhost); // Choose a new random waypoint upon reaching the current one
        }

        // Face the direction of movement
        Vector3 direction = targetWaypoint.position - sGhost.transform.position;
        if (direction.magnitude > 0)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180, 0);
            sGhost.transform.rotation = Quaternion.Slerp(sGhost.transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private void ChooseRandomWaypoint(S_GhostStateManager sGhost)
    {
        if (sGhost.waypoints.Count == 0) return; // No waypoints to choose from

        int randomIndex = Random.Range(0, sGhost.waypoints.Count); // Select a random index
        targetWaypoint = sGhost.waypoints[randomIndex]; // Set the target waypoint
    }

    public override void OnSpiritTriggerEnter(S_GhostStateManager sGhost, Collider collider)
    {
        GameObject other = collider.gameObject;
        if (other.CompareTag("Player"))
        {
            Debug.Log("PLAYER NEARBY RUN");
            sGhost.SwitchState(sGhost.FleeState);
        }
    }

    public override void UseSpiritBox(S_GhostStateManager sGhost)
    {
        // Implement spirit box logic here
    }
}
 