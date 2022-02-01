using UnityEngine;
public class PlayerMenuState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager stateManager)
    {
        stateManager.SetPlayerControls(false);
    }

    public override void ExitState(PlayerStateManager stateManager)
    {

    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (InputManager.Instance.PressedEscape())
            stateManager.SwitchState(stateManager.previousState);
    }
}
