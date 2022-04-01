using UnityEngine;
using Unity.Netcode;
public class PlayerHookState : PlayerBaseState
{
    private PlayerStateManager _stateManager;
    private RodControls _rodControls;
    private ulong _clientID;
    private int _lastCheck;
    private int _biteChance;
    private int _biteTimer;
    private int _biteSucces;
    private NetworkTickSystem _tickSystem;
    private FishBase _currentFish;
    private Transform _hook;

    public override void EnterState(PlayerStateManager stateManager)
    {
        if (!_rodControls)
            _rodControls = stateManager.fishingRod.GetComponent<RodControls>();
        if (_tickSystem == null)
            _tickSystem = stateManager.NetworkManager.NetworkTickSystem;
        if (!_stateManager)
            _stateManager = stateManager;

        _rodControls.enabled = true;
        _rodControls.RotateCamera(stateManager.bobber.transform);
        _clientID = NetworkManager.Singleton.LocalClientId;
        _lastCheck = 0;
        _hook = stateManager.bobber.transform.Find("hook");

        //calulate bite chance and bite timer (hard coded for now until equipment can be added)
        _biteChance = 5;
        _biteTimer = 4 * (int)_tickSystem.TickRate; //the number is how many seconds should pass
        _biteSucces = Random.Range(0, _biteChance);

        _rodControls.reelIn += ReelIn;
        stateManager.NetworkManager.NetworkTickSystem.Tick += CheckBite;
        GameEventManager.Instance.fishHooked.invokedEvent += FishHooked;
    }

    public override void ExitState(PlayerStateManager stateManager)
    {
        stateManager.NetworkManager.NetworkTickSystem.Tick -= CheckBite;
        GameEventManager.Instance.fishHooked.invokedEvent -= FishHooked;
        _rodControls.reelIn -= ReelIn;
        _currentFish = null;
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

    private void FishHooked()
    {
        _stateManager.currentFish = _currentFish;
        _stateManager.SwitchState(_stateManager.playerReelState);
    }

    private void ReelIn(bool fishOn)
    {
        _rodControls.enabled = false;
        _stateManager.SwitchState(_stateManager.playerFishingState);
    }

    private void CheckBite()
    {
        int currentTick = _tickSystem.ServerTime.Tick;

        if (_lastCheck == 0)
            _lastCheck = currentTick;

        if (currentTick - _lastCheck < _biteTimer)
            return;

        GetBiteServerRPC(_clientID, _biteChance);
        _lastCheck = currentTick;
    }

    [ServerRpc(RequireOwnership = false)]
    private void GetBiteServerRPC(ulong clientID, int odds)
    {
        //randomly select a number
        int result = Random.Range(0, odds);

        GetBiteResultClientRPC(clientID, result);
    }

    [ClientRpc]
    private void GetBiteResultClientRPC(ulong clientID, int result)
    {
        if (clientID != NetworkManager.Singleton.LocalClientId)
            return;

        if (result != _biteSucces)
            return;

        Debug.Log("bite!");
        _currentFish = _stateManager.fishManager.GetFish(_hook.position);
        _rodControls.bitingFish = _currentFish;
        GameEventManager.Instance.fishBite.InvokeEvent();
    }
}
