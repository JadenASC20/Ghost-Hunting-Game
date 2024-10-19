

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class S_GhostVacuumedState : S_GhostBaseState
{
    private GhostCapturingDevice capturingDevice;

    public void SetCapturingDevice(GhostCapturingDevice device)
    {
        capturingDevice = device; // Store the reference to the capturing device
    }

    public override void EnterState(S_GhostStateManager sGhost)
    {
        Debug.Log("Ghost is Being Vacuumed...");
    }

    public override void UpdateState(S_GhostStateManager sGhost)
    {
        if (capturingDevice == null) return; // Ensure capturing device exists

        // Move the ghost towards the capturing device's position (itself)
        float step = sGhost.agent.speed * Time.deltaTime; // Calculate distance to move
        sGhost.transform.position = Vector3.MoveTowards(sGhost.transform.position, capturingDevice.transform.position, step);

        // Check if the ghost has reached the capturing device
        if (Vector3.Distance(sGhost.transform.position, capturingDevice.transform.position) < 0.1f)
        {
            // Check if the ghost is within the capturing device's box collider
            if (IsGhostInsideBoxCollider(sGhost, capturingDevice))
            {
                Debug.Log("Ghost has been vacuumed and killed!");
                // Call method to kill the ghost
                sGhost.KillGhost();
            }
            else
            {
                Debug.Log("Ghost did not get captured in time, fleeing...");
                sGhost.SwitchState(sGhost.FleeState); // Switch to flee state
            }
        }
    }

    private bool IsGhostInsideBoxCollider(S_GhostStateManager sGhost, GhostCapturingDevice device)
    {
        // Get the ghost's collider
        Collider ghostCollider = sGhost.GetComponent<Collider>();
        if (ghostCollider != null)
        {
            // Find the box collider on the capturing device
            BoxCollider boxCollider = device.GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                // Check if the ghost's collider intersects with the box collider
                return boxCollider.bounds.Intersects(ghostCollider.bounds);
            }
        }
        return false;
    }

    public override void OnSpiritTriggerEnter(S_GhostStateManager sGhost, Collider collider)
    {
        // Not used, but can be implemented for other interactions
    }

    public override void UseSpiritBox(S_GhostStateManager sGhost)
    {
        // Not used, but can be implemented for other interactions
    }
}

