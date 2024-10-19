
using UnityEngine;
using UnityEngine.AI;

public abstract class S_GhostBaseState : MonoBehaviour
{
    // abstract means we will define their functionality in classes that derive from the sGhost Base State
    public abstract void EnterState(S_GhostStateManager sGhost);
    public abstract void UpdateState(S_GhostStateManager sGhost);
    public abstract void OnSpiritTriggerEnter(S_GhostStateManager sGhost, Collider collider);
    public abstract void UseSpiritBox(S_GhostStateManager sGhost);

}
