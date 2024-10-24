using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class isVacuuming : MonoBehaviour
{
    // The mesh to use for the collider

    // Position and rotation for the collider
    public Vector3 spawnPosition = Vector3.zero;
    public Quaternion spawnRotation = Quaternion.identity;

    public GameObject particlesObject;
    public Material dissolveMaterial; // Assign your material in the inspector
    public GameObject ghostCapture;
    public GameObject ghostCaptureExplosion;
    public float delay = 2.0f; // Delay in seconds

    void Start()
    {
        //SpawnMeshCollider();
    }

    // Update is called once per frame

    //private void SpawnMeshCollider()
    //{
    //    // Create a new GameObject for the mesh collider
    //    GameObject colliderObject = new GameObject("GhostCollider");

    //    // Set position and rotation
    //    colliderObject.transform.position = spawnPosition;
    //    colliderObject.transform.rotation = Quaternion.Euler(0, -180f, 0);


    //    // Add MeshFilter and MeshCollider components
    //    MeshFilter meshFilter = colliderObject.AddComponent<MeshFilter>();
    //    MeshCollider meshCollider = colliderObject.AddComponent<MeshCollider>();

    //    // Assign the mesh
    //    meshFilter.mesh = mesh;
    //    meshCollider.sharedMesh = mesh;

    //    // Enable the collider
    //    meshCollider.convex = true; // Set to true if you need it to detect collisions with dynamic objects
        

    //}

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the tag "Ghost"
        Debug.Log("COLLIDED!!!!!");
        if (other.CompareTag("Ghost"))
        {
            Debug.Log("Ghost detected: " + other.name);
            // essentially vacuuming would be handled here
            // ...

            if (particlesObject != null)
            {
                PlayAllParticleSystems(particlesObject);
                //DocumentCaptureProgress(ghostHealth);
            }
            else
            {
                Debug.LogWarning("Target Object is not assigned.");
            }
        }
    }

    private void PlayAllParticleSystems(GameObject obj)
    {
        // Get all Particle Systems in the children of the specified GameObject
        ParticleSystem[] particleSystems = obj.GetComponentsInChildren<ParticleSystem>();

        // Play each Particle System
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }
    }

    private void DocumentCaptureProgress(float progress)
    {
        ghostCapture.SetActive(true);
        float normalizedFloat = 0.8f - ((progress / 100f) * 0.8f);
        dissolveMaterial.SetFloat("_MyFloat", normalizedFloat);
        if (normalizedFloat >= 0.8f)
        {
            ghostCapture.SetActive(false);
            ghostCaptureExplosion.SetActive(true);
            StartCoroutine(DisableObjectAfterDelay());
        }
    }

    private IEnumerator DisableObjectAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Disable the GameObject
        ghostCaptureExplosion.SetActive(false);
    }
}
