using UnityEngine;
public class PlayerMenuState : PlayerBaseState
{
    private PlayerStateManager _stateManager;

    public override void EnterState(PlayerStateManager stateManager)
    {
        //Do not use! Menu Type must be specified
        Debug.LogError("Menu State was accessed without menu type");
        stateManager.SwitchState(stateManager.playerRoamState);
    }

    public override void ExitState(PlayerStateManager stateManager)
    {
        GameEventManager.Instance.GetGameEvent("ExitMenu").invokedEvent -= ExitMenu;
    }

    public void EnterState(PlayerStateManager stateManager, MenuBaseDocumentLogic menuBase)
    {
        if (!_stateManager)
            _stateManager = stateManager;

        stateManager.SetPlayerControls(false);
        UIManager.Instance.LoadMenu(menuBase);
        GameEventManager.Instance.GetGameEvent("ExitMenu").invokedEvent += ExitMenu;
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (InputManager.Instance.PressedEscape())
            ExitMenu();
    }

    private void ExitMenu()
    {
        UIManager.Instance.UnloadMenu();
        _stateManager.SwitchState(_stateManager.previousState);
    }
}
