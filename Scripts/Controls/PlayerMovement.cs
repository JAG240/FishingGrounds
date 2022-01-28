using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    #region Seralized Vars
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpHeight;
    #endregion

    #region Private Vars
    private InputManager inputManager;
    [SerializeField] private CharacterController characterController;
    private bool _grounded;
    private Vector3 _playerVelocity;
    #endregion

    #region Monobehavior
    //References must be made before this scipt is potentially disabled in start
    private void Awake()
    {
        //Get required references
        inputManager = InputManager.Instance;
        characterController = GetComponent<CharacterController>();
        _playerVelocity = characterController.velocity;
    }

    void Start()
    {
        //If this is another player disable camera and scripts update methods
        if (!IsLocalPlayer)
        {
            //GetComponentInChildren<Camera>().enabled = false;
            enabled = false;
        }
    } 
    //All actions are to be applied to owner client immediately before sending to server to be synced to prevent lag
    void Update()
    {
        //Check if the player is grounded and normalize velocity.y 
        _grounded = characterController.isGrounded;
        if(_grounded && _playerVelocity.y < -0.5)
        {
            NormalizeVelocityServerRPC();
            NormalizeVelocity();
        }

        //Get input values and translate to local space movement 
        Vector2 keyInput = inputManager.GetPlayerMovement();
        Vector3 movement = keyInput.x * transform.right + keyInput.y * transform.forward;

        //Sync movement to other clients and move
        MovePlayerServerRPC(movement);
        ApplyMovement(movement);

        //Checks if player should jump
        //physics.gravity is set in project settings > physics 
        if(inputManager.PressedJump() && _grounded)
        {
            PlayerJumpServerRPC();
            Jump();
        }

        //Sync and apply gravity
        if(!_grounded)
        {
            ApplyGravityServerRPC();
            ApplyGravity();
        }

        //Sync and apply velocity
        ApplyVelocityServerRPC();
        ApplyVelocity();
    }
    #endregion

    #region Movement Logic
    private void NormalizeVelocity()
    {
        _playerVelocity.y = 0;
    }
    private void ApplyMovement(Vector3 movement)
    {
        characterController.Move(movement * Time.deltaTime * _speed);
    }

    private void Jump()
    {
        _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -Physics.gravity.y);
    }

    private void ApplyGravity()
    {
        _playerVelocity.y += Physics.gravity.y * Time.deltaTime;
    }

    private void ApplyVelocity()
    {
        characterController.Move(_playerVelocity * Time.deltaTime);
    }
    #endregion

    #region ServerRPCs
    //All server RPCs would include validation of requests, currently no validation is done as it is not part of MVP 
    [ServerRpc]
    private void NormalizeVelocityServerRPC()
    {
        NormalizeVelocityClientRPC();
    }

    [ServerRpc]
    private void MovePlayerServerRPC(Vector3 movement)
    {
        MovePlayerClientRPC(movement);
    }

    [ServerRpc]
    private void PlayerJumpServerRPC()
    {
        PlayerJumpClientRPC();
    }

    [ServerRpc]
    private void ApplyGravityServerRPC()
    {
        ApplyGravityClientRPC();
    }

    [ServerRpc]
    private void ApplyVelocityServerRPC()
    {
        ApplyVelocityClientRPC();
    }
    #endregion

    #region ClientRPCs
    //Client RPCs only to be called on clients that are not the owner as this action should have been done in the owner's update
    [ClientRpc]
    private void NormalizeVelocityClientRPC()
    {
        if (IsOwner)
            return;

        NormalizeVelocity();
    }

    [ClientRpc]
    private void MovePlayerClientRPC(Vector3 movement)
    {
        if (IsOwner)
            return;

        ApplyMovement(movement);
    }

    [ClientRpc]
    private void PlayerJumpClientRPC()
    {
        if (IsOwner)
            return;

        Jump();
    }

    [ClientRpc]
    private void ApplyGravityClientRPC()
    {
        if (IsOwner)
            return;

        ApplyGravity();
    }

    [ClientRpc]
    private void ApplyVelocityClientRPC()
    {
        if (IsOwner)
            return;

        ApplyVelocity();
    }
    #endregion
}
