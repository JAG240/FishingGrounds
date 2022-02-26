using Unity.Netcode;
using UnityEngine;

/**
 * This class controls which state logic should be running depending on the current state.
 * It also hold the previous state before switching into a menu state so the state
 * can be preserved after exiting a menu. 
 */
public class PlayerStateManager : NetworkBehaviour
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
    public PlayerBaseState previousState { get; private set; }
    public GameObject fishingRod { get; private set; }
    public Bobber bobber { get; private set; }
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
        fishingRod = transform.Find("fishing_rod").gameObject;
        bobber = transform.Find("bobber").GetComponent<Bobber>();
    }

    void Update()
    {
        _currentState.UpdateState(this);
    }
    #endregion

    #region State Logic
    public void SwitchState(PlayerBaseState newState)
    {
        if(newState.Equals(playerMenuState))
            previousState = _currentState.Equals(playerRoamState) ? playerRoamState : playerFishingState;

        _currentState.ExitState(this);
        _currentState = newState;
        _currentState.EnterState(this);
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
