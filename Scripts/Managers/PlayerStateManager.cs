using Unity.Netcode;

public class PlayerStateManager : NetworkBehaviour
{
    #region States
    public PlayerBaseState playerRoamState = new PlayerRoamState();
    public PlayerMenuState playerMenuState = new PlayerMenuState();
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
        if (!IsLocalPlayer)
        {
            enabled = false;
            return;
        }

        _playerMovement = GetComponent<PlayerMovement>();
        _cameraController = GetComponentInChildren<CameraController>();
        _currentState = playerRoamState;
        _currentState.EnterState(this);
        GameEventManager.Instance.GetGameEvent("ExitGame").invokedEvent += ExitGame;
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
        _currentState = newState;
        _currentState.EnterState(this);
    }

    public void SwitchState(PlayerMenuState newState, MenuBaseDocumentLogic menuBase)
    {
        _currentState.ExitState(this);

        if (_currentState.Equals(playerRoamState))
            previousState = playerRoamState;
        else
            previousState = playerFishingState;

        _currentState = newState;
        newState.EnterState(this, menuBase);
    }

    private void ExitGame()
    {
        GameEventManager.Instance.GetGameEvent("ExitGame").invokedEvent -= ExitGame;
        _currentState.ExitState(this);
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
