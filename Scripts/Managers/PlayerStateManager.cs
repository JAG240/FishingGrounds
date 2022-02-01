using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    #region States
    public PlayerBaseState playerRoamState = new PlayerRoamState();
    public PlayerBaseState playerMenuState = new PlayerMenuState();
    public PlayerBaseState playerFishingState = new PlayerFishingState();
    public PlayerBaseState playerCastState = new PlayerCastState();
    public PlayerBaseState playerHookState = new PlayerHookState();
    public PlayerBaseState playerReelState = new PlayerReelState();
    public PlayerBaseState playerUnhookState = new PlayerUnhookState();
    #endregion

    #region Vars
    private PlayerBaseState _currentState;
    public PlayerBaseState previousState;
    private PlayerMovement _playerMovement;
    private CameraController _cameraController;
    #endregion

    #region Monobehaviour
    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _cameraController = GetComponentInChildren<CameraController>();
        _currentState = playerRoamState;
        _currentState.EnterState(this);
    }

    void Update()
    {
        _currentState.UpdateState(this);
    }
    #endregion

    #region State Logic
    public void SwitchState(PlayerBaseState newState)
    {
        _currentState.ExitState(this);

        if (_currentState.Equals(playerRoamState))
            previousState = playerRoamState;
        else
            previousState = playerFishingState;

        _currentState = newState;
        _currentState.EnterState(this);
    }

    public void SetPlayerControls(bool state)
    {
        if(_playerMovement.allowMovement != state)
            _playerMovement.allowMovement = state;

        if(_cameraController.enabled != state)
            _cameraController.enabled = state;
    }
    #endregion
}
