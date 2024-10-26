using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerExitScript : MonoBehaviour
{
    public GameObject canvas; // Reference to the canvas to display the message
    private bool isNearExit = false; // To track if the player is near the exit
    public GameObject player; // Reference to the player GameObject
    public string sceneToLoad;


    // Assume this method checks if the player has completed the objectives
    public bool HasCompletedObjectives()
    {
        // Implement your logic to check if the player has completed the game objectives
        return true; // Placeholder: Replace with actual condition
    }

    private void Start()
    {
        // Ensure the canvas is initially hidden
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
    }

    private void Update()
    {
        // Check for player button press when near the exit
        if (isNearExit && HasCompletedObjectives() && Gamepad.current.buttonSouth.wasPressedThisFrame) // Assuming 'A' button is buttonSouth
        {
            //DespawnPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("COLLIDED WITH EXIT TRIGGER");
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }


}

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isNearExit = false;

            // Hide the canvas when the player leaves the area
            if (canvas != null)
            {
                canvas.SetActive(false);
            }
        }
    }

    private void DespawnPlayer()
    {
        // Logic for despawning the player
        player.SetActive(false); // Optionally deactivate the player GameObject
        Debug.Log("Player has been despawned. Closing application.");

        // Close the application
        Application.Quit();

        // If you are running in the editor, this will stop play mode
        #if UNITY_EDITOR
                            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
