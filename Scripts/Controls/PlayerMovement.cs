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
    private CharacterController characterController;
    private bool _grounded;
    private Vector3 _playerVelocity;
    private Vector3 _gravity;
    #endregion

    #region MonoBehaviour
    //References must be made before this scipt is potentially disabled in start
    void Awake()
    {
        inputManager = InputManager.Instance;
        characterController = GetComponent<CharacterController>();
        _playerVelocity = characterController.velocity;
        _gravity = Physics.gravity;
    }

    void Start()
    {
        //If this is another player disable this scripts update method
        if (!IsLocalPlayer)
            enabled = false;
    } 

    //All actions are to be applied to owner client immediately before sending to server to be synced to prevent lag
    void Update()
    {
        _grounded = characterController.isGrounded;
        if(_grounded && _playerVelocity.y < -0.5)
        {
            NormalizeVelocityServerRPC();
            NormalizeVelocity();
        }

        //Get input values and translate to local space movement 
        Vector2 keyInput = inputManager.GetPlayerMovement();
        Vector3 movement = keyInput.x * transform.right + keyInput.y * transform.forward;

        MovePlayerServerRPC(movement);
        ApplyMovement(movement);

        if(inputManager.PressedJump() && _grounded)
        {
            PlayerJumpServerRPC();
            Jump();
        }

        ApplyGravityServerRPC();
        ApplyGravity();

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
        //Gravity is set in project settings > physics 
        _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -_gravity.y);
    }

    private void ApplyGravity()
    {
        _playerVelocity.y += _gravity.y * Time.deltaTime;
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