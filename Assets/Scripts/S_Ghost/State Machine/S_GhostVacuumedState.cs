

//using System.Collections;
//using UnityEngine;

//public class S_GhostVacuumedState : S_GhostBaseState
//{
//    private S_GhostHealthManager healthManager;
//    private GhostCapturingDevice capturingDevice;
//    private float vacuumTimer;

//    public void SetCapturingDevice(GhostCapturingDevice device)
//    {
//        capturingDevice = device;
//    }

//    public override void EnterState(S_GhostStateManager sGhost)
//    {
//        vacuumTimer = 0;
//        Debug.Log("Ghost is Being Vacuumed...");
//        healthManager = sGhost.ghostHealthManager;

//    }

//    public override void UpdateState(S_GhostStateManager sGhost)
//    {
//        if (capturingDevice == null)
//        {
//            sGhost.SwitchState(sGhost.FleeState);
//            return;
//        }

//        // Move toward the capturing device
//        float step = sGhost.agent.speed * Time.deltaTime;
//        sGhost.transform.position = Vector3.MoveTowards(sGhost.transform.position, capturingDevice.transform.position, step);


//        // Check if the ghost is close enough to the capturing device
//        if (Vector3.Distance(sGhost.transform.position, capturingDevice.transform.position) < 0.1f)
//        {
//            if (IsGhostInsideBoxCollider(sGhost, capturingDevice))
//            {
//                Debug.Log("Ghost is inside the capturing device's collider.");
//                healthManager.DecreaseHealth();
//                // Check if the ghost has health
//                if (healthManager != null && healthManager.CurrentHealth <= 0)
//                {
//                    Debug.Log("Ghost has been vacuumed and killed!");
//                    //healthManager.KillGhost(); // Kill the ghost if health is depleted
//                    //GhostManager.GhostKilled(); // Ensure GhostManager is referenced correctly
//                    sGhost.KillGhost();
//                    GhostManager.GhostKilled();
//                    //sGhost.currentHealth = maxHealth;
//                }
//            }
//        }
//    }



//    private bool IsGhostInsideBoxCollider(S_GhostStateManager sGhost, GhostCapturingDevice device)
//    {
//        Collider ghostCollider = sGhost.GetComponent<Collider>();
//        BoxCollider boxCollider = device.GetComponent<BoxCollider>();
//        return boxCollider != null && ghostCollider != null && boxCollider.bounds.Intersects(ghostCollider.bounds);
//    }

//    public override void OnSpiritTriggerEnter(S_GhostStateManager sGhost, Collider collider) { }

//    public override void UseSpiritBox(S_GhostStateManager sGhost) { }
//}

using System.Collections;
using UnityEngine;

public class S_GhostVacuumedState : S_GhostBaseState
{
    private S_GhostHealthManager healthManager;
    private MonoBehaviour capturingDevice; // Store either GhostCapturingDevice or VrGhostCapture
    private float vacuumTimer;

    public void SetCapturingDevice(MonoBehaviour device)
    {
        capturingDevice = device;
    }

    public override void EnterState(S_GhostStateManager sGhost)
    {
        vacuumTimer = 0;
        Debug.Log("Ghost is Being Vacuumed...");
        healthManager = sGhost.ghostHealthManager;

        // If no device is set, check for one
        if (capturingDevice == null)
        {
            TryFindCapturingDevice(sGhost);
        }
    }

    public override void UpdateState(S_GhostStateManager sGhost)
    {
        if (capturingDevice == null)
        {
            sGhost.SwitchState(sGhost.FleeState);
            return;
        }

        // Move toward the capturing device
        Transform deviceTransform = capturingDevice.transform;
        float step = sGhost.agent.speed * Time.deltaTime;
        sGhost.transform.position = Vector3.MoveTowards(sGhost.transform.position, deviceTransform.position, step);

        // Check if the ghost is close enough to the capturing device
        if (Vector3.Distance(sGhost.transform.position, deviceTransform.position) < 0.1f)
        {
            if (IsGhostInsideBoxCollider(sGhost, deviceTransform))
            {
                Debug.Log("Ghost is inside the capturing device's collider.");
                healthManager.DecreaseHealth();
                // Check if the ghost has health
                if (healthManager != null && healthManager.CurrentHealth <= 0)
                {
                    Debug.Log("Ghost has been vacuumed and killed!");
                    sGhost.KillGhost();
                    GhostManager.GhostKilled();
                }
            }
        }
    }

    private void TryFindCapturingDevice(S_GhostStateManager sGhost)
    {
        // Check for GhostCapturingDevice
        GhostCapturingDevice device = FindObjectOfType<GhostCapturingDevice>();
        if (device != null)
        {
            capturingDevice = device;
            Debug.Log("Found GhostCapturingDevice.");
            return;
        }

        // Check for VrGhostCapture
        VrGhostCapture vrDevice = FindObjectOfType<VrGhostCapture>();
        if (vrDevice != null)
        {
            capturingDevice = vrDevice;
            Debug.Log("Found VrGhostCapture.");
            return;
        }

        Debug.Log("No capturing device found.");
    }

    private bool IsGhostInsideBoxCollider(S_GhostStateManager sGhost, Transform deviceTransform)
    {
        Collider ghostCollider = sGhost.GetComponent<Collider>();
        BoxCollider boxCollider = deviceTransform.GetComponent<BoxCollider>();
        return boxCollider != null && ghostCollider != null && boxCollider.bounds.Intersects(ghostCollider.bounds);
    }

    public override void OnSpiritTriggerEnter(S_GhostStateManager sGhost, Collider collider) { }

    public override void UseSpiritBox(S_GhostStateManager sGhost) { }
}
