using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Make sure to include this for UI elements
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

    public SoundManager soundManager;
    private SphereCollider sphereCollider;
    public float sphereRadius = 5f;

    [SerializeField]
    private Image cooldownImage; // Reference to the UI Image for cooldown

    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(UseSpiritBox);

        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = sphereRadius;
        sphereCollider.enabled = false;

        cooldownImage.fillAmount = 0f; // Initialize fill amount
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
        }
        else
        {
            failureVoiceparticleSystem.Play();
            soundManager.PlayRandomSound("spiritBoxError");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }
}
