

using System.Collections;
using UnityEngine;

public class GhostCapturingDevice : MonoBehaviour
{
    public float sphereRadius = 10f; // Radius of the sphere collider
    private SphereCollider sphereCollider;

    private void Start()
    {
        // Initialize the sphere collider but keep it disabled for now
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = sphereRadius;
        sphereCollider.enabled = false; // Initially disabled
    }

    private void Update()
    {
        // Check if the "G" key is held down
        if (Input.GetKey(KeyCode.G))
        {
            if (!sphereCollider.enabled) // Only activate if it's not already enabled
            {
                Debug.Log("Capturing Device Activated");
                ActivateSphereCollider();
            }

            // Check for ghosts within the sphere collider while the key is held
            CheckForGhosts();
        }
        else
        {
            // Disable the sphere collider when the key is released
            if (sphereCollider.enabled)
            {
                StartCoroutine(DisableColliderAfterDelay(0f)); // Disable immediately
                StartCoroutine(DestroyAfterDelay(0f)); // Destroy after a delay
            }
        }
    }

    private void ActivateSphereCollider()
    {
        sphereCollider.enabled = true; // Enable the sphere collider
        Debug.Log("Sphere Collider Activated!");
    }

    private void CheckForGhosts()
    {
        // Get all colliders within the sphere's radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Ghost"))
            {
                S_GhostStateManager ghostManager = hitCollider.GetComponent<S_GhostStateManager>();
                if (ghostManager != null)
                {
                    // Switch the ghost's state to vacuumed
                    ghostManager.SwitchState(ghostManager.VacuumedState);
                    ghostManager.VacuumedState.SetCapturingDevice(this); // Set the capturing device reference
                    Debug.Log($"Ghost {hitCollider.gameObject.name} is now being vacuumed.");
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            other.gameObject.layer = 0; // Set to layer 0
            Debug.Log($"Ghost {other.gameObject.name} entered and switched to layer 0.");
        }
    }


    private IEnumerator DisableColliderAfterDelay(float delay)
    {
        sphereCollider.enabled = false; // Disable the collider
        Debug.Log("Sphere Collider Deactivated!");
        yield return null; // Just wait one frame
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        Debug.Log("Destroying Capturing Device");
        Destroy(gameObject); // Destroy the capturing device
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the sphere in the editor for visualization
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }
}
