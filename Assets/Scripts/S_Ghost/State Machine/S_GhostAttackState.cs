using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class S_GhostAttackState : S_GhostBaseState
{
    public override void EnterState(S_GhostStateManager sGhost)
    {
        Debug.Log("Ghost is Attacking...");
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
}
