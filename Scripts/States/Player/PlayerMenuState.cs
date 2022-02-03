using UnityEngine;
public class PlayerMenuState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager stateManager)
    {
        //Do not use! Menu Type must be specified
        Debug.LogError("Menu State was accessed without menu type");
        stateManager.SwitchState(stateManager.playerRoamState);
    }

    public override void ExitState(PlayerStateManager stateManager)
    {
        //UIManager.Instance.SetPauseMenu(false);
    }

    public void EnterState(PlayerStateManager stateManager, MenuBaseDocumentLogic menuBase)
    {
        stateManager.SetPlayerControls(false);
        UIManager.Instance.LoadMenu(menuBase);
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (InputManager.Instance.PressedEscape())
            ExitMenu(stateManager);
    }

    private void ExitMenu(PlayerStateManager stateManager)
    {
        UIManager.Instance.UnloadMenu();
        stateManager.SwitchState(stateManager.previousState);
    }
}
