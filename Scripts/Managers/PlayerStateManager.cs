using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    #region Public States
    public PlayerBaseState playerRoamState = new PlayerRoamState();
    public PlayerBaseState playerMenuState = new PlayerMenuState();
    #endregion

    #region Vars
    private PlayerBaseState _currentState;
    private PlayerMovement _playerMovement;
    private CameraController _cameraController;
    #endregion

    void Start()
    {
        _currentState = playerRoamState;
        _currentState.EnterState(this);
        _playerMovement = GetComponent<PlayerMovement>();
        _cameraController = GetComponentInChildren<CameraController>();
    }

    void Update()
    {
        _currentState.UpdateState(this);
    }

    public void SwitchState(PlayerBaseState newState)
    {
        _currentState.ExitState(this);
        _currentState = newState;
        _currentState.EnterState(this);
    }

    public void DisablePlayerControls()
    {
        _playerMovement.allowMovement = false;
        _cameraController.enabled = false;
    }

    public void EnablePlayerControls()
    {
        _playerMovement.allowMovement = true;
        _cameraController.enabled = true;
    }
}
