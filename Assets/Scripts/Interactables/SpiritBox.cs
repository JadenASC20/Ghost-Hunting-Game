//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.XR.Interaction.Toolkit;

//public class SpiritBox : MonoBehaviour
//{
//    [SerializeField]
//    private ParticleSystem successVoiceparticleSystem;
//    [SerializeField]
//    private ParticleSystem failureVoiceparticleSystem;

//    public float cooldownDuration = 5f;
//    public float scanDuration = 3f;
//    private bool isOnCooldown = false;
//    private bool ghostFound;

//    private SphereCollider sphereCollider;
//    public float sphereRadius = 5f;

//    [SerializeField]
//    private Image cooldownImage; // Reference to the UI Image for cooldown

//    // Add this line
//    [SerializeField]
//    private S_GhostSoundDatabase soundDatabase; // Reference to the sound database

//    private AudioSource audioSource; // Reference for playing sounds

//    // Add this variable for the index of the sound to play
//    public int soundIndexToPlay = 0; // Default index

//    void Start()
//    {
//        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
//        grabbable.activated.AddListener(UseSpiritBox);

//        sphereCollider = gameObject.AddComponent<SphereCollider>();
//        sphereCollider.isTrigger = true;
//        sphereCollider.radius = sphereRadius;
//        sphereCollider.enabled = false;

//        cooldownImage.fillAmount = 0f; // Initialize fill amount

//        // Initialize the audio source
//        audioSource = gameObject.AddComponent<AudioSource>();
//    }

//    public void UseSpiritBox(ActivateEventArgs arg)
//    {
//        if (!isOnCooldown)
//        {
//            StartCoroutine(ScanForGhost());
//        }
//    }

//    private IEnumerator ScanForGhost()
//    {
//        isOnCooldown = true;
//        ghostFound = false;
//        cooldownImage.fillAmount = 1f; // Reset fill amount

//        // Check for ghosts within the sphere
//        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereRadius);
//        foreach (var hitCollider in hitColliders)
//        {
//            if (hitCollider.CompareTag("Ghost"))
//            {
//                Debug.Log("Ghost Found!!!!");
//                ghostFound = true;
//                break;
//            }
//        }

//        PlayParticleSystem(ghostFound);

//        // Cooldown
//        float elapsedTime = 0f;
//        while (elapsedTime < cooldownDuration)
//        {
//            elapsedTime += Time.deltaTime;
//            cooldownImage.fillAmount = 1 - (elapsedTime / cooldownDuration); // Update fill amount
//            yield return null; // Wait for the next frame
//        }

//        isOnCooldown = false;
//        cooldownImage.fillAmount = 0f; // Reset fill amount after cooldown
//    }

//    private void PlayParticleSystem(bool success)
//    {
//        if (success)
//        {
//            successVoiceparticleSystem.Play();
//        }
//        else
//        {
//            failureVoiceparticleSystem.Play();
//            PlayNormalSoundAtIndex(soundIndexToPlay); // Play the sound at the specified index
//        }
//    }

//    private void PlayNormalSoundAtIndex(int index)
//    {
//        if (soundDatabase != null && soundDatabase.normalSounds.Length > index)
//        {
//            // Select the sound at the specified index
//            AudioClip selectedSound = soundDatabase.normalSounds[index];
//            audioSource.clip = selectedSound;
//            audioSource.Play();
//        }
//        else
//        {
//            Debug.LogWarning("Sound database is not assigned or index is out of bounds!");
//        }
//    }

//    private void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.black;
//        Gizmos.DrawWireSphere(transform.position, sphereRadius);
//    }
//}


using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class SpiritBox : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem successVoiceparticleSystem;
    [SerializeField]
    private ParticleSystem failureVoiceparticleSystem;

    public float cooldownDuration = 5f;
    public float scanDuration = 3f;
    private bool isOnCooldown = false;
    private bool ghostFound;

    private SphereCollider sphereCollider;
    public float sphereRadius = 5f;

    [SerializeField]
    private Image cooldownImage; // Reference to the UI Image for cooldown

    // Reference to the sound database
    [SerializeField]
    private S_GhostSoundDatabase soundDatabase;

    private AudioSource audioSource; // Reference for playing sounds

    // Indexes for sound playback
    public int soundIndexOnSuccess = 0; // Sound index to play on success
    public int soundIndexOnFailure = 0; // Sound index to play on failure

    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(UseSpiritBox);

        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = sphereRadius;
        sphereCollider.enabled = false;

        cooldownImage.fillAmount = 0f; // Initialize fill amount

        // Initialize the audio source
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void UseSpiritBox(ActivateEventArgs arg)
    {
        if (!isOnCooldown)
        {
            StartCoroutine(ScanForGhost());
        }
    }

    private IEnumerator ScanForGhost()
    {
        isOnCooldown = true;
        ghostFound = false;
        cooldownImage.fillAmount = 1f; // Reset fill amount

        // Check for ghosts within the sphere
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Ghost"))
            {
                Debug.Log("Ghost Found!!!!");
                ghostFound = true;
                break;
            }
        }

        PlayParticleSystem(ghostFound);

        // Cooldown
        float elapsedTime = 0f;
        while (elapsedTime < cooldownDuration)
        {
            elapsedTime += Time.deltaTime;
            cooldownImage.fillAmount = 1 - (elapsedTime / cooldownDuration); // Update fill amount
            yield return null; // Wait for the next frame
        }

        isOnCooldown = false;
        cooldownImage.fillAmount = 0f; // Reset fill amount after cooldown
    }

    private void PlayParticleSystem(bool success)
    {
        if (success)
        {
            successVoiceparticleSystem.Play();
            PlayNormalSoundAtIndex(soundIndexOnSuccess); // Play the success sound
        }
        else
        {
            failureVoiceparticleSystem.Play();
            PlayNormalSoundAtIndex(soundIndexOnFailure); // Play the failure sound
        }
    }

    private void PlayNormalSoundAtIndex(int index)
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }
}
