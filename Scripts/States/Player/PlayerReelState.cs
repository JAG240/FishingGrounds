using UnityEngine;
public class PlayerReelState : PlayerBaseState
{
    private RodControls _rodControls;
    public override void EnterState(PlayerStateManager stateManager)
    {
        Debug.Log("Hooked Fish!");
        
        if (!_rodControls)
            _rodControls = stateManager.fishingRod.GetComponent<RodControls>();
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
