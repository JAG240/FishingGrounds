using UnityEngine;
public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerStateManager stateManager);
    public abstract void UpdateState(PlayerStateManager statManager);
    public abstract void ExitState(PlayerStateManager stateManager);
}