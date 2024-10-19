

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_GhostFleeState : S_GhostBaseState
{
    private float safeDistance = 25.0f; // Minimum distance from the player to the waypoint
    private Transform playerPosition;
    private float enemyDistanceRun = 4.0f; // Distance from player to trigger fleeing
    private float speed = 15f; // Movement speed of the ghost
    private float waypointChangeCooldown = 0.5f; // Cooldown time to prevent jitter
    private float lastWaypointChangeTime = 0f; // Last time the waypoint was changed
    private Vector3 directionToWaypoint; // Store direction to waypoint for reference

    public override void EnterState(S_GhostStateManager sGhost)
    {
        Debug.Log("Ghost is Fleeing...");
        playerPosition = sGhost.Player.transform;
        CalculateRandomWaypoint(sGhost);
    }

    public override void OnSpiritTriggerEnter(S_GhostStateManager sGhost, Collider collider)
    {
        // Handle other trigger interactions if necessary
    }

    public override void UseSpiritBox(S_GhostStateManager sGhost)
    {
        // Spirit box logic
    }

    public override void UpdateState(S_GhostStateManager sGhost)
    {
        if (sGhost.Player != null && sGhost.chosenWaypoint != null)
        {
            float distanceToPlayer = Vector3.Distance(sGhost.transform.position, sGhost.Player.transform.position);
            directionToWaypoint = sGhost.chosenWaypoint.position - sGhost.transform.position;

            // If the player is too close, calculate a new direction away from the player
            if (distanceToPlayer < enemyDistanceRun)
            {
                Debug.Log("Player is too close! Fleeing in the opposite direction...");

                // Calculate a direction away from the player
                Vector3 fleeDirection = (sGhost.transform.position - sGhost.Player.transform.position).normalized;
                // Add some randomness to the flee direction
                fleeDirection += new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;

                // Move the ghost in the flee direction
                sGhost.transform.position += fleeDirection * speed * Time.deltaTime;

                // Face the flee direction
                Quaternion lookRotation = Quaternion.LookRotation(fleeDirection);
                sGhost.transform.rotation = Quaternion.Slerp(sGhost.transform.rotation, lookRotation, Time.deltaTime * 5f);

                // Recalculate the waypoint
                CalculateRandomWaypoint(sGhost);
                return; // Exit to avoid moving towards the waypoint
            }

            // Move toward the chosen waypoint
            sGhost.transform.position += directionToWaypoint.normalized * speed * Time.deltaTime;

            // Face the direction of movement
            if (directionToWaypoint.magnitude > 0)
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToWaypoint);
                sGhost.transform.rotation = Quaternion.Slerp(sGhost.transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            // Check if the ghost has reached the chosen waypoint
            if (Vector3.Distance(sGhost.transform.position, sGhost.chosenWaypoint.position) < 0.1f)
            {
                Debug.Log("Ghost has reached the chosen waypoint. Switching to Patrol State.");
                sGhost.SwitchState(sGhost.PatrolState);
            }

            // Check if the chosen waypoint is too close to the player
            if (Vector3.Distance(playerPosition.position, sGhost.chosenWaypoint.position) <= safeDistance)
            {
                Debug.Log("Chosen waypoint is too close to the player! Recalculating...");
                CalculateRandomWaypoint(sGhost);
            }
        }
        else
        {
            Debug.LogWarning("Player reference or chosen waypoint is null.");
        }
    }

    private void CalculateRandomWaypoint(S_GhostStateManager sGhost)
    {
        if (sGhost.waypoints.Count == 0)
        {
            Debug.LogError("No waypoints assigned!");
            return;
        }

        // Randomly select a waypoint
        Transform randomWaypoint;
        do
        {
            randomWaypoint = sGhost.waypoints[Random.Range(0, sGhost.waypoints.Count)];
        }
        while (Vector3.Distance(playerPosition.position, randomWaypoint.position) <= safeDistance);

        sGhost.chosenWaypoint = randomWaypoint;
        Debug.Log($"Moving to waypoint: {randomWaypoint.name}");
    }
}

