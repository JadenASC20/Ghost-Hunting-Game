using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXVacuumSucking : MonoBehaviour
{
    public VisualEffect vfx; // Reference to the Visual Effect component
    public string eventName = "PlayEffect"; // The event name to trigger the effect
    public KeyCode triggerKey = KeyCode.Space; // The key to hold down

    private bool isPlaying = false;

    private void Start()
    {
        vfx = GetComponent<VisualEffect>();
    }

    void Update()
    {
        // Check if the trigger key is pressed
        if (Input.GetKeyDown(triggerKey))
        {
            PlayVFX();
        }

        // Check if the trigger key is held down
        if (Input.GetKey(triggerKey))
        {
            PauseVFX();
        }

        // Check if the trigger key is released
        if (Input.GetKeyUp(triggerKey))
        {
            ResumeVFX();
        }
    }

    void PlayVFX()
    {
        if (!isPlaying)
        {
            vfx.SendEvent(eventName); // Trigger the VFX
            isPlaying = true;
        }
    }

    void PauseVFX()
    {
        if (isPlaying)
        {
            // Set a parameter in the VFX to pause it
            vfx.SetFloat("PlaybackSpeed", 0f); // Assuming PlaybackSpeed is used to control speed
        }
    }

    void ResumeVFX()
    {
        if (isPlaying)
        {
            // Resume the VFX by restoring playback speed
            vfx.SetFloat("PlaybackSpeed", 1f); // Assuming normal speed is 1
        }
    }
}
