using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class XRSceneManager : MonoBehaviour
{
    // Method to change to a specified scene by name
    public void ChangeScene(string sceneName)
    {
        // Check if the scene is in the build settings
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene " + sceneName + " cannot be loaded. Check the build settings.");
        }
    }

    // Optionally, you can also implement a method to change scene by index
    public void ChangeScene(int sceneIndex)
    {
        // Check if the index is valid
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError("Scene index " + sceneIndex + " is out of range.");
        }
    }
}
