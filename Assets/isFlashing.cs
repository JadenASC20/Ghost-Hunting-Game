using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isFlashing : MonoBehaviour
{
    public float sphereRadius = 5f; // Radius of the sphere collider
    private SphereCollider sphereCollider;

    // Reference to the sound database
    public S_GhostSoundDatabase soundDatabase; // Assign this in the inspector
    private AudioSource audioSource;

    // Index to select a specific sound from the normalSounds array
    public int soundIndex = 0; // Default index

    private void OnEnable()
    {
        // Initialize the sphere collider
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true; // Set as a trigger
        sphereCollider.radius = sphereRadius; // Set the radius
        sphereCollider.enabled = false; // Initially disabled

        // Initialize the audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        ToggleCollider();
    }

    private void ToggleCollider()
    {
        sphereCollider.enabled = !sphereCollider.enabled;

        if (sphereCollider.enabled)
        {
            Debug.Log("Sphere Collider Activated!");
            PlaySoundAtIndex(soundIndex); // Play sound at the specified index

            CheckForObjectsInVicinity();
            // Optionally destroy after a short duration
            //Destroy(gameObject, 2f); // Destroys the flasher after 2 seconds
        }
        else
        {
            Debug.Log("Sphere Collider Deactivated!");
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
