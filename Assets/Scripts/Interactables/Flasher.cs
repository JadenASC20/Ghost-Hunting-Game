//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//public class Flasher : MonoBehaviour
//{
//    public float sphereRadius = 5f; // Radius of the sphere collider
//    private SphereCollider sphereCollider;

//    private void Start()
//    {
//        // Initialize the sphere collider
//        sphereCollider = gameObject.AddComponent<SphereCollider>();
//        sphereCollider.isTrigger = true; // Set as a trigger
//        sphereCollider.radius = sphereRadius; // Set the radius
//        sphereCollider.enabled = false; // Initially disabled
//    } // can keep this 

//    private void Update()
//    {
//        // Toggle the sphere collider with the "F" key
//        if (Input.GetKeyDown(KeyCode.F))
//        {
//            Debug.Log("Flasher Used");
//            ToggleCollider();
//        }
//    }

//    private void ToggleCollider()
//    {
//        sphereCollider.enabled = !sphereCollider.enabled;

//        if (sphereCollider.enabled)
//        {
//            Debug.Log("Sphere Collider Activated!");
//            CheckForObjectsInVicinity();
//            // Destroy afterwards
//        }
//        else
//        {
//            Debug.Log("Sphere Collider Deactivated!");
//        }
//    }

//    private void CheckForObjectsInVicinity()
//    {
//        // Check for objects within the collider's radius
//        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereRadius);
//        foreach (var hitCollider in hitColliders)
//        {
//            if (hitCollider.CompareTag("Ghost"))
//            {
//                // Get the state manager
//                S_GhostStateManager sGhost = hitCollider.GetComponent<S_GhostStateManager>();
//                if (sGhost != null)
//                {
//                    // Set the stun power multiplier
//                    sGhost.stunPowerMultiplier = 2f;
//                    StartCoroutine(StunCoroutine(sGhost));
//                }
//            }
//        }
//    }

//    private void OnDrawGizmosSelected()
//    {
//        // Draw the sphere in the editor for visualization
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawWireSphere(transform.position, sphereRadius);
//    }

//    private IEnumerator StunCoroutine(S_GhostStateManager sGhost)
//    {
//        if (sGhost.agent == null)
//        {
//            Debug.LogError("NavMeshAgent is null!");
//            yield break; // Exit if the agent is null
//        }

//        // Stop the NavMeshAgent
//        sGhost.agent.isStopped = true;
//        sGhost.agent.velocity = Vector3.zero;

//        // Wait for the stun duration based on the stun power multiplier
//        yield return new WaitForSeconds(sGhost.stunPowerMultiplier);

//        // Resume NavMeshAgent movement
//        sGhost.agent.isStopped = false;

//        // Switch to stun state
//        sGhost.SwitchState(sGhost.StunState);

//        Debug.Log("Ghost is no longer stunned!");
//    }
//}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Flasher : MonoBehaviour
{
    public float sphereRadius = 5f; // Radius of the sphere collider
    private SphereCollider sphereCollider;

    private void Start()
    {
        // Initialize the sphere collider
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true; // Set as a trigger
        sphereCollider.radius = sphereRadius; // Set the radius
        sphereCollider.enabled = false; // Initially disabled
    }

    private void Update()
    {
        // Toggle the sphere collider with the "F" key
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Flasher Used");
            ToggleCollider();
        }
    }

    private void ToggleCollider()
    {
        sphereCollider.enabled = !sphereCollider.enabled;

        if (sphereCollider.enabled)
        {
            Debug.Log("Sphere Collider Activated!");
            CheckForObjectsInVicinity();
            // Optionally destroy after a short duration
            Destroy(gameObject, 2f); // Destroys the flasher after 2 seconds
        }
        else
        {
            Debug.Log("Sphere Collider Deactivated!");
        }
    }

    private void CheckForObjectsInVicinity()
    {
        // Check for objects within the collider's radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Ghost"))
            {
                // Get the state manager
                S_GhostStateManager sGhost = hitCollider.GetComponent<S_GhostStateManager>();
                if (sGhost != null)
                {
                    // Set the stun power multiplier if needed
                    sGhost.stunPowerMultiplier = 2f; // This can be adjusted based on your logic

                    // Switch to the stun state immediately
                    sGhost.SwitchState(sGhost.StunState);
                    Debug.Log("Ghost has been stunned!");
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the sphere in the editor for visualization
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }
}
