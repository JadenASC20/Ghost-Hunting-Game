using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class isVacuuming : MonoBehaviour
{
    // The mesh to use for the collider

    // Position and rotation for the collider
    public Transform attractPoint;
    public Vector3 spawnPosition = Vector3.zero;
    public Quaternion spawnRotation = Quaternion.identity;

    public GameObject particlesObject;
    public Material dissolveMaterial; // Assign your material in the inspector
    public GameObject ghostCapture;
    public GameObject ghostCaptureExplosion;
    public float delay = 2.0f; // Delay in seconds

    private SphereCollider sphereCollider;
    private float sphereRadius = 2f;

    void OnEnable()
    {
        SpawnVacuumCollider();
    }

    private void OnDisable()
    {
        DeleteVacuumCollider();
        
    }

    void Update()
    {
        CheckforGhosts();
    }


    private void CheckforGhosts()
    {
        // Check for ghosts within the sphere
        //Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereRadius);
        //foreach (var hitCollider in hitColliders)
        //{
        //    if (hitCollider.CompareTag("Ghost"))
        //    {
        //        // essentially vacuuming would be handled here
        //        // ...

        //        if (particlesObject != null)
        //        {
        //            particlesObject.SetActive(true);
        //            PlayAllParticleSystems(particlesObject);
        //            //DocumentCaptureProgress(ghostHealth);
        //        }
        //        else
        //        {
        //            Debug.LogWarning("Target Object is not assigned.");
        //        }

        //        //S_GhostStateManager ghostManager = hitCollider.GetComponent<S_GhostStateManager>();
        //        //if (ghostManager != null)
        //        //{
        //        //    // Switch the ghost's state to vacuumed
        //        //    ghostManager.SwitchState(ghostManager.VacuumedState);
        //        //    //ghostManager.VacuumedState.SetCapturingDevice(this); // Set the capturing device reference

        //        //    Debug.Log($"Ghost {hitCollider.gameObject.name} is now being vacuumed.");
        //        //}

        //        Debug.Log("Ghost Found!!!!");
        //        break;
        //    }
        //    else
        //    {
        //        particlesObject.SetActive(false);
        //    }
        //}
    }

    // Update is called once per frame

    private void SpawnVacuumCollider()
    {
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = sphereRadius;
        sphereCollider.enabled = true;
        sphereCollider.center = new Vector3(0f, 1.7f, 0f);
    }
    private void DeleteVacuumCollider()
    {
        if (sphereCollider != null)
        {
            Destroy(sphereCollider);
            Debug.Log("SphereCollider deleted.");
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(sphereCollider.center, sphereRadius);
    }
}
