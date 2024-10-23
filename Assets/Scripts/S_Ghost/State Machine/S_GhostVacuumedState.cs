using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class S_GhostVacuumedState : S_GhostBaseState
{
    private GhostCapturingDevice capturingDevice;
    private GhostManager ghostManager;

    public void SetCapturingDevice(GhostCapturingDevice device)
    {
        capturingDevice = device; // Store the reference to the capturing device
    }

    public void SetGhostManager(GhostManager manager)
    {
        ghostManager = manager; // Store the reference to the ghost manager
    }

    public override void EnterState(S_GhostStateManager sGhost)
    {
        Debug.Log("Ghost is Being Vacuumed...");
    }

    public override void UpdateState(S_GhostStateManager sGhost)
    {
        if (capturingDevice == null)
        {
            sGhost.SwitchState(sGhost.FleeState);
            return; 
        }

        float step = sGhost.agent.speed * Time.deltaTime;
        sGhost.transform.position = Vector3.MoveTowards(sGhost.transform.position, capturingDevice.transform.position, step);

        if (Vector3.Distance(sGhost.transform.position, capturingDevice.transform.position) < 0.1f)
        {
            if (IsGhostInsideBoxCollider(sGhost, capturingDevice))
            {
                Debug.Log("Ghost has been vacuumed and killed!");
                sGhost.KillGhost();

                // Call the method to update the ghost count
                //ghostManager?.GhostKilled();

                GhostManager.GhostKilled();

                Debug.Log("Cakked GhostManager.GhostKilled");
            }
        }
    }



    private bool IsGhostInsideBoxCollider(S_GhostStateManager sGhost, GhostCapturingDevice device)
    {
        Collider ghostCollider = sGhost.GetComponent<Collider>();
        if (ghostCollider != null)
        {
            BoxCollider boxCollider = device.GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                return boxCollider.bounds.Intersects(ghostCollider.bounds);
            }
        }
        return false;
    }

    public override void OnSpiritTriggerEnter(S_GhostStateManager sGhost, Collider collider) { }

    public override void UseSpiritBox(S_GhostStateManager sGhost) { }
}
