using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDatabase", menuName = "ScriptableObjects/SoundDatabase", order = 1)]
public class S_GhostSoundDatabase : ScriptableObject
{
    public AudioClip[] normalSounds;  // Sounds when not using a spirit box -- in the same vicinity of the ghost
    public AudioClip[] spiritBoxSoundPatrolling;  // Sounds when using a spirit box -- Patrolling
    public AudioClip[] spiritBoxError;  // Sounds when attempting to use a spirit box but failing
    public AudioClip[] musicBox;
    public AudioClip[] ghostCapturingDevice;
    public AudioClip[] flasher;

}
