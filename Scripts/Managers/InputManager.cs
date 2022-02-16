using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Vars
    private static InputManager _instance;
    public static InputManager Instance { get { return _instance; } }
    private PlayerControls _playerControls;
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        //Singleton logic
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        //As OnDisable is called before script is destroyed, we must check if object exists
        if (_playerControls != null)
            _playerControls.Disable();
    }
    #endregion

    #region Read Input Methods
    //x (-1 is left, 1 is right), y (-1 is backwards, 1 is forwards)
    public Vector2 GetPlayerMovement()
    {
        return _playerControls.Player.Movement.ReadValue<Vector2>();
    }

    //x (-1 is  look down, 1 is look up), y (-1 is look left, 1 is look right)
    public Vector2 GetMouseDelta()
    {
        return _playerControls.Player.Look.ReadValue<Vector2>();
    }

    //returns true if the key was pressed in this frame
    public bool PressedJump()
    {
        return _playerControls.Player.Jump.triggered;
    }

    public bool PressedEscape()
    {
        return _playerControls.Player.Escape.triggered;
    }

    public bool ToggledRod()
    {
        return _playerControls.Player.ToggleRod.triggered;
    }

    //Left mouse button
    public bool LeftAction()
    {
        return _playerControls.Player.LeftAction.triggered;
    }

    //Right mouse button
    public bool RightAction()
    {
        return _playerControls.Player.RightAction.triggered;
    }

    public bool LeftActionHeld()
    {
        return _playerControls.Player.LeftAction.IsPressed();
    }
    
    public bool RightActionHeld()
    {
        return _playerControls.Player.RightAction.IsPressed();
    }
    #endregion
}
