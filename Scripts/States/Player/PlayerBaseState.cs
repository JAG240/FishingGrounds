using UnityEngine;

/**
 * This abstract class will ensure that all player states have the ability
 * to run logic when the state is entered, exited, and once every frame on the update loop. 
 */
public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerStateManager stateManager);
    public abstract void ExitState(PlayerStateManager stateManager);
    public abstract void UpdateState(PlayerStateManager stateManager);
}
