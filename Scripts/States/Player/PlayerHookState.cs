using UnityEngine;
public class PlayerHookState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager stateManager)
    {
        stateManager.fishingRod.transform.localPosition = new Vector3(0,-0.45f,1);
    }

    public override void ExitState(PlayerStateManager stateManager)
    {
        stateManager.fishingRod.transform.localPosition = new Vector3(0.5f, -0.1f, 0.2f);
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (InputManager.Instance.PressedEscape())
        {
            UIManager.Instance.LoadUIDocument(new PauseMenuDocumentLogic());
            stateManager.SwitchState(stateManager.playerMenuState);
            return;
        }
    }
}
