using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeScript : MonoBehaviour
{
    // This Script Handles the Ghost Movment
    // Attach to the ghost itself
    // Will have the booleans Wandering and RunAway as Serialized booleans as well as a prefab floor for it to wander on
    // Serialized boolean for speed as well
    // If in Wandering mode will wander aimlesssly around the bounds of the tile
    // If in contact with player, or some other possible condition that will change later on, will activate run away mode.
    // Run away mode will send the ghost in the opposite direction it was traveling in before (away from suck) and keep it going to the legth of double the floor

    // Serialized Fields for Wandering, RunAway, speed, and the floor the ghost can wander on
    // All the givens because why not
    [SerializeField] private bool Wandering = true;
    [SerializeField] private bool RunAway = false;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private GameObject floor;

    [SerializeField] private bool IsSucked = false; // set to true if vacum sucks this ghost

    private int RunAwaySpeedMultiplier = 2; // made to speed up the ghost in runaway mode
    private int RunAwayDistanceMultiplier = 3; // made to dictate how far away the ghost should run away

    private Vector3 targetPosition; // The position the ghost moves towards
    private Vector3 direction; // Direction of movement
    private Bounds floorBounds; // To get the bounds of the floor
    private float floorLength;


    private void Start()
    {
        // Get the bounds of the floor object to constrain the wandering movement
        if (floor != null)
        {
            floorBounds = floor.GetComponent<Collider>().bounds;
            floorLength = floorBounds.size.x; // assuming square floor
        }

        // Set initial wandering target position
        SetRandomWanderPosition();
    }

    private void Update()
    {
        // handles the cases where Wandering and Runaway are active/not
        if (Wandering && !RunAway)
        {
            Wander();
        }

        if (RunAway)
        {
            RunAwayFromPlayer();
        }
    }

    // Handes the Wandering the ghost does in relation to the given floor prefab
    private void Wander()
    {
        // Move the ghost towards the target position
        direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Check if the ghost has reached its target position and set a new one
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetRandomWanderPosition();
        }
    }

    private void SetRandomWanderPosition()
    {
        // Generate a random position within the floor bounds for the ghost to wander to
        float randomX = Random.Range(floorBounds.min.x, floorBounds.max.x);
        float randomZ = Random.Range(floorBounds.min.z, floorBounds.max.z);

        targetPosition = new Vector3(randomX, transform.position.y, randomZ);
    }

    private void RunAwayFromPlayer()
    {
        // Run away in the opposite direction of the last movement
        direction = (transform.position - targetPosition).normalized;
        transform.position += direction * speed * Time.deltaTime * RunAwaySpeedMultiplier;

        // Continue running away until it has traveled twice the length of the floor
        if (Vector3.Distance(transform.position, targetPosition) > floorLength * RunAwayDistanceMultiplier)
        {
            RunAway = false; // Stop running away after the condition is met
            Wandering = true; // Return to wandering mode
        }
    }


    /*
    Important! Important! Important! Important! Important! Important! Important! Important! Important! Important! Important! Important! Important!
    Important! Important! Important! Important! Important! Important! Important! Important! Important! Important! Important! Important! Important!
    Important! Important! Important! Important! Important! Important! Important! Important! Important! Important! Important! Important! Important!
    Important! Important! Important! Important! Important! Important! Important! Important! Important! Important! Important! Important! Important!
    Important! Important! Important! Important! Important! Important! Important! Important! Important! Important! Important! Important! Important!
    Important! Important! Important! Important! Important! Important! Important! Important! Important! Important! Important! Important! Important!
    */
    // This function should be changed in reaction of a direction change from the vacum. Would be good if there was a boolean to use.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Switch to run away mode when contacting the player
            RunAway = true;
            Wandering = false;
            targetPosition = transform.position; // Store the last position before running away
        }
    }
}
