using UnityEngine;
public class PlayerMenuState : PlayerBaseState
{
    private PlayerStateManager _stateManager;

    public override void EnterState(PlayerStateManager stateManager)
    {
        if (!_stateManager)
            _stateManager = stateManager;

        stateManager.SetPlayerControls(false);
        GameEventManager.Instance.GetGameEvent("ExitMenu").invokedEvent += ExitMenu;
    }

    public override void ExitState(PlayerStateManager stateManager)
    {
        GameEventManager.Instance.GetGameEvent("ExitMenu").invokedEvent -= ExitMenu;
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (InputManager.Instance.PressedEscape())
            ExitMenu();
    }

    private void ExitMenu()
    {
        UIManager.Instance.UnloadUIDocument();
        _stateManager.SwitchState(_stateManager.previousState);
    }
}
