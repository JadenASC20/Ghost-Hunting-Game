
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public static GhostManager Instance;

    // Static variable to hold the count of killed ghosts
    public static int totalGhostsKilled = 0;

    // Reference to the sound database
    public S_GhostSoundDatabase soundDatabase; // Assign this in the inspector
    private AudioSource audioSource;

    // Index to select a specific sound from the normalSounds array
    public int deathSoundIndex = 0; // Default index for death sound

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        // Initialize the audio source
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Method to notify a ghost was killed
    public static void GhostKilled()
    {
        Debug.Log("in ghostkilled function");
        totalGhostsKilled++;
        Debug.Log($"Ghost Killed incremented. Total: {totalGhostsKilled}");

        // Play sound on ghost kill
        Instance.PlaySoundAtIndex(Instance.deathSoundIndex);
    }

    public static int GetTotalGhostsKilled()
    {
        return totalGhostsKilled;
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
}
