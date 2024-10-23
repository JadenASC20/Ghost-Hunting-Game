using UnityEngine;

public class GhostManager : MonoBehaviour
{

    public static GhostManager Instance;

    // Static variable to hold the count of killed ghosts
    public static int totalGhostsKilled = 0;

    

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
}

    // Method to notify a ghost was killed
    public static void GhostKilled()
    {
        Debug.Log("in ghostkilled funciton");
        totalGhostsKilled++;
        Debug.Log($"Ghost Killed incremented. Total: {totalGhostsKilled}");
    }
    public static int GetTotalGhostsKilled()
    {
    return totalGhostsKilled;
    }

}
