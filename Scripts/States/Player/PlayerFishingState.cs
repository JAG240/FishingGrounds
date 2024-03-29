using UnityEngine;
public class PlayerFishingState : PlayerBaseState
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
        if (InputManager.Instance.PressedEscape())
        {
            UIManager.Instance.LoadUIDocument(new PauseMenuDocumentLogic());
            stateManager.SwitchState(stateManager.playerMenuState);
            return;
        }

        if(InputManager.Instance.ToggledRod())
        {
            stateManager.SwitchState(stateManager.playerRoamState);
            return;
        }

        if(InputManager.Instance.Cast())
        {
            stateManager.SwitchState(stateManager.playerCastState);
            return;
        }
    }
}
