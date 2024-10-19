using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostCapturingDevice : MonoBehaviour
{
    public float sphereRadius = 5f; // Radius of the sphere collider
    private SphereCollider sphereCollider;

    private void Start() // can keep this I guess
    {
        // Initialize the sphere collider but keep it disabled for now
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = sphereRadius;
        sphereCollider.enabled = false; // Initially disabled
    }

    private void Update()
    {
        // Check for the "G" key press to activate the sphere collider
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Capturing Device Activated");
            ActivateSphereCollider();
        }
    }

    private void ActivateSphereCollider()
    {
        sphereCollider.enabled = true; // Enable the sphere collider
        Debug.Log("Sphere Collider Activated!");

        // Check for ghosts within the sphere collider
        CheckForGhosts();

        // Disable the collider again after a short delay
        StartCoroutine(DisableColliderAfterDelay(1f)); // Adjust duration as needed

        // Destroy GameObject
    }

    private void CheckForGhosts()
    {
        // Get all colliders within the sphere's radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Ghost"))
            {
                // TODO: CHANGE THIS FROM INSTEAD THE NEW LOGIC WE IMPLEMENTED
                NavMeshAgent ghostAgent = hitCollider.GetComponent<NavMeshAgent>();
                if (ghostAgent != null)
                {
                    // Set the destination of the ghost to the position of this GameObject
                    ghostAgent.SetDestination(transform.position);
                    Debug.Log($"Ghost {hitCollider.gameObject.name} is now moving to the capturing device.");
                }
            }
        }
    }

    private IEnumerator DisableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        sphereCollider.enabled = false; // Disable the collider after the delay
        Debug.Log("Sphere Collider Deactivated!");
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the sphere in the editor for visualization
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }
}
