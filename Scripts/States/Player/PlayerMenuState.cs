using UnityEngine;
public class PlayerMenuState : PlayerBaseState
{
    private PlayerStateManager _stateManager;

    public override void EnterState(PlayerStateManager stateManager)
    {
        if (!_stateManager)
            _stateManager = stateManager;

        if(stateManager.bobber.isActiveAndEnabled)
            stateManager.bobber.Return();

        stateManager.SetPlayerControls(false);
        GameEventManager.Instance.exitMenu.invokedEvent += ExitMenu;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public override void ExitState(PlayerStateManager stateManager)
    {
        GameEventManager.Instance.exitMenu.invokedEvent -= ExitMenu;
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (InputManager.Instance.PressedEscape())
            GameEventManager.Instance.exitMenu.InvokeEvent();
    }

    private void ExitMenu()
    {
        UIManager.Instance.UnloadUIDocument();
        _stateManager.SwitchState(_stateManager.previousState);
    }
}
