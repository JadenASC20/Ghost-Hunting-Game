
using UnityEngine;

public class S_GhostStunState : S_GhostBaseState
{
    public float stunDuration = 1f; // Duration of the stun effect
    private float stunTimer; // Timer to track stun duration

    // Flag to indicate if the ghost is currently stunned
    private bool isStunned = false;

    public override void EnterState(S_GhostStateManager sGhost)
    {
        Debug.Log("Ghost is Stunned...");
        stunTimer = stunDuration;
        isStunned = true; // Set the stunned flag

        // Play stunned animation
        PlayStunnedAnimation(sGhost);

        // Disable actions while stunned
        DisableActions(sGhost);
    }

    public override void UpdateState(S_GhostStateManager sGhost)
    {
        if (!isStunned) return; // Skip update if not stunned

        // Update the stun timer
        stunTimer -= Time.deltaTime;

        // Check if stun duration has elapsed
        if (stunTimer <= 0f)
        {
            Debug.Log("Ghost is no longer stunned.");
            EnableActions(sGhost); // Re-enable actions after stun ends
            isStunned = false; // Reset the stunned flag
            sGhost.SwitchState(sGhost.FleeState); // Change this as needed
        }
    }

    public override void OnSpiritTriggerEnter(S_GhostStateManager sGhost, Collider collider)
    {
        // Handle any specific behavior when a spirit trigger is entered while stunned
        Debug.Log("Spirit trigger entered while stunned.");
    }

    public override void UseSpiritBox(S_GhostStateManager sGhost)
    {
        // Handle the spirit box logic during stun if needed
        Debug.Log("Spirit box used while ghost is stunned.");
    }

    // Method to play the stunned animation
    private void PlayStunnedAnimation(S_GhostStateManager sGhost)
    {
        Animator animator = sGhost.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Stunned"); // Adjust this to your animation trigger
        }
    }

    // Method to disable actions while stunned
    private void DisableActions(S_GhostStateManager sGhost)
    {
        // Implement logic to disable ghost actions (e.g., movement)
        Debug.Log("Actions disabled while ghost is stunned.");
    }

    // Method to enable actions after stun ends
    private void EnableActions(S_GhostStateManager sGhost)
    {
        // Implement logic to re-enable ghost actions
        Debug.Log("Actions re-enabled after stun.");
    }
}
