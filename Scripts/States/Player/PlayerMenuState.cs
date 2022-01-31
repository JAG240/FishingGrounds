using UnityEngine;
public class PlayerMenuState : PlayerBaseState
{
    private InputManager _inputManager;

    public override void EnterState(PlayerStateManager stateManager)
    {
        if (!_inputManager)
            _inputManager = InputManager.Instance;

        stateManager.DisablePlayerControls();
    }

    public override void ExitState(PlayerStateManager stateManager)
    {
        stateManager.EnablePlayerControls();
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (_inputManager.PressedEscape())
            stateManager.SwitchState(stateManager.playerRoamState);
    }
}
