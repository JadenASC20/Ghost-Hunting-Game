using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private S_GhostSoundDatabase soundDatabase;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayRandomSound(string soundType)
    {
        AudioClip[] clips = GetSoundArray(soundType);
        if (clips != null && clips.Length > 0)
        {
            AudioClip clipToPlay = clips[Random.Range(0, clips.Length)];
            audioSource.PlayOneShot(clipToPlay);
        }
        else
        {
            Debug.LogWarning($"No sound clips found for type: {soundType}");
        }
    }

    private AudioClip[] GetSoundArray(string soundType)
    {
        switch (soundType.ToLower())
        {
            case "normalSounds":
                return soundDatabase.normalSounds;
            case "spiritBoxSoundPatrolling":
                return soundDatabase.spiritBoxSoundPatrolling;
            case "spiritBoxError":
                return soundDatabase.spiritBoxError;
            case "musicBox":
                return soundDatabase.musicBoxSounds;
            case "ghostCapturingDevice":
                return soundDatabase.ghostCapturingDevice;
            case "flasher":
                return soundDatabase.flasher;
            default:
                Debug.LogWarning($"Sound type '{soundType}' is not recognized.");
                return null;
        }
    }
}
