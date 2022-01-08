using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance { get { return _instance; } }
    private PlayerControls playerControls;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        //If there is more than one input manager destroy it, else store singleton instance
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        //Create new player controls
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    //x (-1 is left, 1 is right), y (-1 is backwards, 1 is forwards)
    public Vector2 GetPlayerMovement()
    {
        return playerControls.Player.Movement.ReadValue<Vector2>();
    }

    //x (-1 is  look down, 1 is look up), y (-1 is look left, 1 is look right)
    public Vector2 GetMouseDelta()
    {
        return playerControls.Player.Look.ReadValue<Vector2>();
    }

    //returns true if the key was pressed in this frame
    public bool PressedJump()
    {
        return playerControls.Player.Jump.triggered;
    }

    public bool PressedEscape()
    {
        return playerControls.Player.Escape.triggered;
    }
}
