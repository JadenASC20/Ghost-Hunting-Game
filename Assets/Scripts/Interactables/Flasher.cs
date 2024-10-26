using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Flasher : MonoBehaviour
{
    public float sphereRadius = 5f; // Radius of the sphere collider
    private SphereCollider sphereCollider;

    // Reference to the sound database
    public S_GhostSoundDatabase soundDatabase; // Assign this in the inspector
    private AudioSource audioSource;

    // Index to select a specific sound from the normalSounds array
    public int soundIndex = 0; // Default index
    private bool isPlaying = false; // Flag to check if a sound is currently playing

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;

    private void Awake()
    {
        InitializeComponents();
        grabInteractable.selectExited.AddListener(OnThrow);
    }

    private void OnDestroy()
    {
        grabInteractable.selectExited.RemoveListener(OnThrow);
    }

    private void InitializeComponents()
    {
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = sphereRadius;
        sphereCollider.enabled = false;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.5f; // Set fixed volume
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 0) // Check for "Default" layer
        {
            Debug.Log("Flasher Activated");
            ActivateSphereCollider();
        }
    }

    private void ActivateSphereCollider()
    {
        sphereCollider.enabled = true;
        Debug.Log("Sphere Collider Activated!");

        if (!isPlaying)
        {
            PlaySoundAtIndex(soundIndex);
        }

        CheckForObjectsInVicinity();
        StartCoroutine(DisableColliderAfterDelay(1f));
    }

    private IEnumerator DisableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        sphereCollider.enabled = false;
        Debug.Log("Sphere Collider Deactivated!");
    }

    private void PlaySoundAtIndex(int index)
    {
        if (soundDatabase != null && soundDatabase.normalSounds.Length > index)
        {
            audioSource.clip = soundDatabase.normalSounds[index];
            audioSource.Play();
            isPlaying = true;

            StartCoroutine(ResetPlayingFlag(audioSource.clip.length));
        }
        else
        {
            Debug.LogWarning("Sound database is not assigned or index is out of bounds!");
        }
    }

    private IEnumerator ResetPlayingFlag(float delay)
    {
        yield return new WaitForSeconds(delay);
        isPlaying = false;
        Destroy(gameObject); // Optionally destroy the flasher
    }

    private void CheckForObjectsInVicinity()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Ghost"))
            {
                HandleGhost(hitCollider);
            }
        }
    }

    private void HandleGhost(Collider ghostCollider)
    {
        S_GhostStateManager sGhost = ghostCollider.GetComponent<S_GhostStateManager>();
        if (sGhost != null)
        {
            sGhost.stunPowerMultiplier = 2f;
            sGhost.SwitchState(sGhost.StunState);
            Debug.Log("Ghost has been stunned!");
        }
    }

    private void OnThrow(SelectExitEventArgs arg)
    {
        // Handle logic after the object is thrown, if needed
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }
}
