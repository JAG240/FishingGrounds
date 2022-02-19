using UnityEngine;
public class PlayerRoamState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager stateManager)
    {
        stateManager.SetPlayerControls(true);
    }

    public override void ExitState(PlayerStateManager stateManager)
    {

    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (InputManager.Instance.ToggledRod())
        {
            stateManager.SwitchState(stateManager.playerFishingState);
            return;
        }
    }
}
