using UnityEngine;
public class PlayerHookState : PlayerBaseState
{
    private RodControls _rodControls;

    public override void EnterState(PlayerStateManager stateManager)
    {
        if (!_rodControls)
            _rodControls = stateManager.fishingRod.GetComponent<RodControls>();

        _rodControls.enabled = true;
        _rodControls.RotateCamera(stateManager.bobber.transform);
    }

    public override void ExitState(PlayerStateManager stateManager)
    {
        
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (InputManager.Instance.PressedEscape())
        {
            _rodControls.enabled = false;
            UIManager.Instance.LoadUIDocument(new PauseMenuDocumentLogic());
            stateManager.SwitchState(stateManager.playerMenuState);
            return;
        }
    }
}
