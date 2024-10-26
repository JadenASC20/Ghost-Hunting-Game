using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject
    public Transform spawnLocation; // Reference to the spawn location

    private void Start()
    {
        // Spawn the player at the spawn location
        if (player != null && spawnLocation != null)
        {
            player.transform.position = spawnLocation.position;
            player.transform.rotation = spawnLocation.rotation; // Optional: Set rotation
        }
    }
}
