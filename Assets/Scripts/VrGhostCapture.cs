using System.Collections;
using UnityEngine;

public class VrGhostCapture : MonoBehaviour
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

    private void Start()
    {
        // Initialize the sphere collider
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = sphereRadius;

        // Initialize the audio source
        audioSource = gameObject.AddComponent<AudioSource>();

        // Automatically activate the sphere collider on spawn
        ActivateSphereCollider();
    }

    private void Update()
    {
        // Follow the controller's position and rotation
        if (transform.parent != null)
        {
            transform.position = transform.parent.position;
            transform.rotation = transform.parent.rotation;
        }

        // Check for ghosts within the sphere collider
        CheckForGhosts();

        // If the sphere collider is active, check for destruction
        if (hasActivated && !audioSource.isPlaying)
        {
            StartCoroutine(DestroyAfterDelay(0f)); // Destroy immediately if sound has ended
        }
    }


    private void ActivateSphereCollider()
    {
        sphereCollider.enabled = true; // Enable the sphere collider
        Debug.Log("Sphere Collider Activated!");

        // Play activation sound only once
        if (!hasActivated)
        {
            PlaySoundAtIndex(activationSoundIndex); // Play activation sound
            hasActivated = true; // Set the flag to true
        }
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
