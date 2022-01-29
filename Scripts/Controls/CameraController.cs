using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CameraController : NetworkBehaviour
{
    #region Serialized Vars
    [SerializeField] private float _verticalSpeed = 25f;
    [SerializeField] private float _horizontalSpeed = 0.1f;
    #endregion

    #region Private Vars
    private Transform _player;
    private float _yRotation = 0f;
    #endregion

    #region MonoBehaviour
    void Awake()
    {
        //Must get player reference before being disabled in start
        _player = transform.parent.gameObject.transform;
    }

    void Start()
    {
        //TO DO: read mouse sensitivity options from a settings page

        if (!IsLocalPlayer)
        {
            GetComponent<Camera>().enabled = false;
            enabled = false;
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        //TO DO: manage this scripts enabled state depending on player's state
        if (InputManager.Instance.PressedEscape())
            enabled = false;

        Vector2 mouseInput = InputManager.Instance.GetMouseDelta();

        //subtracted to give the feeling of non-inverted camera controls
        //clampped to stop the player from looking beyond physical limits upward or downward
        _yRotation -= mouseInput.y * Time.deltaTime * _verticalSpeed;
        _yRotation = Mathf.Clamp(_yRotation, -90f, 90f);

        ApplyYRotationServerRPC(_yRotation);
        ApplyYRotation(_yRotation);

        ApplyXRotationServerRPC(mouseInput.x);
        ApplyXRotation(mouseInput.x);
    }
    #endregion

    #region Camera Movement Logic
    private void ApplyYRotation(float yRotation)
    {
        transform.localEulerAngles = new Vector3(yRotation, 0f, 0f);
    }

    private void ApplyXRotation(float xRotation)
    {
        _player.Rotate(Vector3.up * xRotation * _horizontalSpeed);
    }
    #endregion

    #region Server RPCs
    //Validation skipped here as we assume client authoritive behavior to meet MVP 
    [ServerRpc]
    private void ApplyYRotationServerRPC(float yRotation)
    {
        ApplyYRotationClientRPC(yRotation);
    }

    [ServerRpc]
    private void ApplyXRotationServerRPC(float xRotation)
    {
        ApplyXRotationClientRPC(xRotation);
    }
    #endregion

    #region Client RPCs
    [ClientRpc]
    private void ApplyYRotationClientRPC(float yRotation)
    {
        if (IsOwner)
            return;

        ApplyYRotation(yRotation);
    }

    [ClientRpc]
    private void ApplyXRotationClientRPC(float xRotation)
    {
        if (IsOwner)
            return;

        ApplyXRotation(xRotation);
    }
    #endregion
}
