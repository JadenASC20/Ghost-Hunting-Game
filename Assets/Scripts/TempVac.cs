

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempVac : MonoBehaviour
{
    public InputActionReference gripInputActionReference;
    public InputActionReference triggerInputActionReference;

    public GameObject isVacuuming;
    public GameObject isFlashing;

    private float _gripValue;
    private float _triggerValue;

    public S_GhostSoundDatabase soundDatabase; // Assign this in the inspector
    private AudioSource audioSource;

    public int flasherSoundIndex = 0; // Sound index for activation
    public int vacuumSoundIndex = 0;

    public GameObject vrGhostCapturePrefab; // Assign this in the inspector
    private GameObject activeVrGhostCapture; // Reference to the active capturing device

    private bool hasActivated = false;

    private void OnEnable()
    {
        gripInputActionReference.action.performed += OnGripPerformed;
        triggerInputActionReference.action.performed += OnTriggerPerformed;
        gripInputActionReference.action.canceled += OnGripCanceled;
        triggerInputActionReference.action.canceled += OnTriggerCanceled;

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnDisable()
    {
        gripInputActionReference.action.performed -= OnGripPerformed;
        triggerInputActionReference.action.performed -= OnTriggerPerformed;
        gripInputActionReference.action.canceled -= OnGripCanceled;
        triggerInputActionReference.action.canceled -= OnTriggerCanceled;

        hasActivated = false;
        StopSound();
    }

    private void OnGripPerformed(InputAction.CallbackContext context)
    {
        _gripValue = context.ReadValue<float>();
        HandleGripAction(_gripValue);
    }

    private void OnTriggerPerformed(InputAction.CallbackContext context)
    {
        _triggerValue = context.ReadValue<float>();
        HandleTriggerAction(_triggerValue);
    }

    private void OnGripCanceled(InputAction.CallbackContext context)
    {
        HandleGripAction(0);
    }

    private void OnTriggerCanceled(InputAction.CallbackContext context)
    {
        HandleTriggerAction(0);
    }

    private void HandleGripAction(float value)
    {
        if (value > 0.2)
        {
            isFlashing.SetActive(true);
            if (!hasActivated)
            {
                hasActivated = true;
                PlaySoundAtIndex(flasherSoundIndex);
            }
        }
        else
        {
            hasActivated = false;
            isFlashing.SetActive(false);
        }
    }

    //private void HandleTriggerAction(float value)
    //{
    //    if (value > 0)
    //    {
    //        isVacuuming.SetActive(true);
    //        if (activeVrGhostCapture == null) // Spawn if not already active
    //        {
    //            activeVrGhostCapture = Instantiate(vrGhostCapturePrefab, transform.position, transform.rotation);
    //            activeVrGhostCapture.transform.SetParent(transform); // Make it a child of the controller
    //            Debug.Log("Spawned VrGhostCapture.");
    //        }

    //        if (!hasActivated)
    //        {
    //            PlaySoundAtIndex(vacuumSoundIndex);
    //            hasActivated = true;
    //        }
    //    }
    //    else
    //    {
    //        isVacuuming.SetActive(false);
    //        hasActivated = false;
    //        StopSound();
    //        if (activeVrGhostCapture != null)
    //        {
    //            Destroy(activeVrGhostCapture); // Destroy the capturing device
    //            activeVrGhostCapture = null; // Reset reference
    //        }
    //    }
    //}

    private void HandleTriggerAction(float value)
    {
        if (value > 0.2)
        {
            isVacuuming.SetActive(true);

            if (activeVrGhostCapture == null) // Spawn if not already active
            {
                // Find the AnchorPoint in the vacuum prefab
                Transform anchorPoint = transform.Find("AnchorPoint");
                if (anchorPoint != null)
                {
                    // Instantiate the VrGhostCapture prefab at the AnchorPoint position and rotation
                    activeVrGhostCapture = Instantiate(vrGhostCapturePrefab, anchorPoint.position, anchorPoint.rotation);
                    activeVrGhostCapture.transform.SetParent(anchorPoint); // Set the parent to AnchorPoint
                    Debug.Log("Spawned VrGhostCapture at AnchorPoint.");
                }
                else
                {
                    Debug.LogWarning("AnchorPoint not found in the TempVac prefab!");
                }
            }

            if (!hasActivated)
            {
                PlaySoundAtIndex(vacuumSoundIndex);
                hasActivated = true;
            }
        }
        else
        {
            isVacuuming.SetActive(false);
            hasActivated = false;
            StopSound();

            if (activeVrGhostCapture != null)
            {
                Destroy(activeVrGhostCapture); // Destroy the capturing device
                activeVrGhostCapture = null; // Reset reference
            }
        }
    }


    private void PlaySoundAtIndex(int index)
    {
        if (soundDatabase != null && soundDatabase.normalSounds.Length > index)
        {
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
}
