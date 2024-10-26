using System.Collections;
using UnityEngine;

public class GhostCapturingDevice : MonoBehaviour
{
    public float sphereRadius = 10f; // Radius of the sphere collider
    private SphereCollider sphereCollider;

    // Reference to the sound database
    public S_GhostSoundDatabase soundDatabase; // Assign this in the inspector
    private AudioSource audioSource;

    // Index for the activation sound clip
    public int activationSoundIndex = 0; // Sound index for activation

    // Flag to check if the activation sound has been played
    private bool hasActivated = false;

    private void Awake()
    {
        Activate();
    }

    private void Activate()
    {
        // Initialize the sphere collider
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = sphereRadius;
        sphereCollider.enabled = false; // Initially disabled

        // Initialize the audio source
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        // Check for active ghosts while the sphere collider is enabled
        if (sphereCollider.enabled)
        {
            CheckForGhosts();
        }
    }

    private void ActivateSphereCollider()
    {
        sphereCollider.enabled = true; // Enable the sphere collider
        Debug.Log("Sphere Collider Activated!");

        // Play activation sound
        PlaySoundAtIndex(activationSoundIndex);
        hasActivated = true; // Set the flag to true

        // Start the timer for destruction after 10 seconds
        StartCoroutine(DestroyAfterDelay(10f));
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

    private void PlaySoundAtIndex(int index)
    {
        if (soundDatabase != null && soundDatabase.normalSounds.Length > index)
        {
            // Select the sound at the specified index
            AudioClip selectedSound = soundDatabase.normalSounds[index];
            audioSource.clip = selectedSound;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Sound database is not assigned or index is out of bounds!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the device collides with the ground (default layer)
        if (collision.gameObject.layer == 0) // Default layer
        {
            Debug.Log("Capturing Device Activated by Ground Collision");
            ActivateSphereCollider(); // Activate the sphere collider
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        Debug.Log("Destroying Capturing Device after 10 seconds");
        Destroy(gameObject); // Destroy the capturing device
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the sphere in the editor for visualization
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }
}
