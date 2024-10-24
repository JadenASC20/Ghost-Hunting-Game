
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

    private void Start()
    {
        // Initialize the sphere collider but keep it disabled for now
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = sphereRadius;
        sphereCollider.enabled = false; // Initially disabled

        // Initialize the audio source
        audioSource = gameObject.AddComponent<AudioSource>();
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
            // Disable the sphere collider and reset sound flags when the key is released
            if (sphereCollider.enabled)
            {
                StartCoroutine(DisableColliderAfterDelay(0f)); // Disable immediately
                StartCoroutine(DestroyAfterDelay(0f)); // Destroy after a delay
            }

            // Reset sound play flag
            hasActivated = false;
            StopSound(); // Stop any currently playing sounds
        }

        // Check if the "I" key is held down for destruction after sound ends
        if (Input.GetKey(KeyCode.I) && hasActivated && !audioSource.isPlaying)
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

    private void StopSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
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
