using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionSceneTransition : MonoBehaviour
{
    // Start is called before the first frame update
    public string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        
        // Check if the other object has the "Marker" tag
        if (other.CompareTag("Marker"))
        {
            // Load the specified scene
            Debug.Log("FOUND MARKER!");
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
