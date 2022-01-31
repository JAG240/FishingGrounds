using UnityEngine;
public class PlayerRoamState : PlayerBaseState
{
    private InputManager _inputManager;

    public override void EnterState(PlayerStateManager stateManager)
    {
        if(!_inputManager)
            _inputManager = InputManager.Instance;
    }

    public override void ExitState(PlayerStateManager stateManager)
    {

    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if(_inputManager.PressedEscape())
            stateManager.SwitchState(stateManager.playerMenuState);
    }
}
