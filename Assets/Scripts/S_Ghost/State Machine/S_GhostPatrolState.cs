

using TMPro;
using UnityEngine;

public class S_GhostPatrolState : S_GhostBaseState
{
    private float patrolRadius = 2f; // Adjust for circle size
    private float patrolSpeed = 2f; // Speed during patrol
    private float patrolAngle = 0f; // Used for circular movement

    public override void EnterState(S_GhostStateManager sGhost)
    {
        Debug.Log("Ghost is Patrolling...");
        patrolAngle = Random.Range(0f, 360f); // Start at a random angle
    }

    public override void UpdateState(S_GhostStateManager sGhost)
    {
        if (sGhost.chosenWaypoint != null)
        {
            // Calculate the new patrol position in a circle
            patrolAngle += Time.deltaTime * patrolSpeed; // Increment angle
            float xOffset = Mathf.Cos(patrolAngle) * patrolRadius;
            float zOffset = Mathf.Sin(patrolAngle) * patrolRadius;

            Vector3 patrolPosition = sGhost.chosenWaypoint.position + new Vector3(xOffset, 0, zOffset);
            sGhost.transform.position = Vector3.MoveTowards(sGhost.transform.position, patrolPosition, patrolSpeed * Time.deltaTime);

            // Face the direction of movement
            Vector3 direction = patrolPosition - sGhost.transform.position;
            if (direction.magnitude > 0)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                sGhost.transform.rotation = Quaternion.Slerp(sGhost.transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
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
