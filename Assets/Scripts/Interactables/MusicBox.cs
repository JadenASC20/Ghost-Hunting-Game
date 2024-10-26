using System.Collections;
using UnityEngine;

public class MusicBox : MonoBehaviour
{
    public float sphereRadius = 5f; // Radius of the sphere collider
    private SphereCollider sphereCollider;

    // Reference to the sound database
    public S_GhostSoundDatabase soundDatabase; // Assign this in the inspector
    private AudioSource audioSource;

    // Flag to check if a sound is currently playing
    private bool isPlaying = false;

    // Reference to the player
    public Transform player; // Assign this in the inspector
    public float maxDistance = 5f; // Maximum distance to hear the sound

    // Fixed volume for the sound
    public float fixedVolume = 0.5f; // Set this in the inspector or code

    private void Activate()
    {
        // Initialize the sphere collider but keep it disabled for now
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = sphereRadius;
        sphereCollider.enabled = false; // Initially disabled

        // Initialize the audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = fixedVolume; // Set the fixed volume for the AudioSource
    }

    private void Update()
    {
        // Volume adjustment can be removed if not needed
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object collided with the ground layer
        if (collision.gameObject.layer == 0)
        {
            Debug.Log("Capturing Device Activated");
            ActivateSphereCollider();
        }
    }

    private void ActivateSphereCollider()
    {
        sphereCollider.enabled = true; // Enable the sphere collider
        Debug.Log("Sphere Collider Activated!");

        // Play a random activation sound only if no sound is currently playing
        if (!isPlaying)
        {
            PlayRandomSound(); // No volume parameter needed
        }

        // Check for ghosts within the sphere collider
        CheckForGhosts();

        // Disable the collider again after a short delay
        StartCoroutine(DisableColliderAfterDelay(1f)); // Adjust duration as needed
    }

    private void PlayRandomSound()
    {
        if (soundDatabase != null && soundDatabase.musicBoxSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, soundDatabase.musicBoxSounds.Length);
            AudioClip selectedSound = soundDatabase.musicBoxSounds[randomIndex];
            audioSource.clip = selectedSound;
            audioSource.Play();
            isPlaying = true; // Set the flag to indicate a sound is playing
            Debug.Log($"Playing sound: {selectedSound.name}");
            StartCoroutine(ResetPlayingFlag(selectedSound.length)); // Reset flag after sound finishes
        }
        else
        {
            Debug.LogWarning("Sound database is not assigned or has no sounds!");
        }
    }


    private IEnumerator ResetPlayingFlag(float delay)
    {
        yield return new WaitForSeconds(delay);
        isPlaying = false; // Reset the flag after the sound has finished
                           // Optionally reset volume here, but it's not necessary if fixedVolume is set
        Destroy(this.gameObject); // Corrected line
    }

    private void CheckForGhosts()
    {
        // Get all colliders within the sphere's radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Ghost"))
            {
                Debug.Log($"Ghost {hitCollider.gameObject.name} detected!");
                // Add your ghost handling logic here
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
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }
}
