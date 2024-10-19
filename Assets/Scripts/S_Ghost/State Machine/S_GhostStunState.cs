using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class S_GhostStunState : S_GhostBaseState
{
    public float stunDuration = 3f; // Duration of the stun effect
    public override void EnterState(S_GhostStateManager sGhost)
    {
        Debug.Log("Ghost is Stunned...");
        sGhost.SwitchState(sGhost.FleeState); // will be changed later

        // play stunned animation
        // wait for seconds * the power of the stun
        // switch into vacuum state if player vacuums

    }

    public override void UpdateState(S_GhostStateManager sGhost)
    {

    }

    public override void OnSpiritTriggerEnter(S_GhostStateManager sGhost, Collider collider)
    {

    }

    public override void UseSpiritBox(S_GhostStateManager sGhost)
    {

    }

    public void OnStunned(S_GhostStateManager sGhost)
    {
        sGhost.SwitchState(sGhost.FleeState); // will be changed later

    }


}

