using UnityEngine;
public class PlayerCastState : PlayerBaseState
{
    private GameObject _castBarObj;
    private CastBar _castBar;
    private bool _casting = false;
    private float _maxBarFill;
    private PlayerStateManager _stateManager;

    public override void EnterState(PlayerStateManager stateManager)
    {
        if(!_castBarObj)
        {
            _castBarObj = stateManager.fishingRod.transform.Find("CastBar").gameObject;
            _castBar = _castBarObj.GetComponent<CastBar>();
        }

        if (!_stateManager)
            _stateManager = stateManager;

        stateManager.SetPlayerControls(false);
        _castBarObj.SetActive(true);
        _castBar.ClearBar();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        stateManager.bobber.exitCast += ExitCastSuccessfully;
        stateManager.bobber.Return();
    }

    public override void ExitState(PlayerStateManager stateManager)
    {
        _castBarObj.SetActive(false);
        stateManager.bobber.exitCast -= ExitCastSuccessfully;
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (InputManager.Instance.PressedEscape())
        {
            _casting = false;
            UIManager.Instance.LoadUIDocument(new PauseMenuDocumentLogic());
            stateManager.SwitchState(stateManager.playerMenuState);
            return;
        }

        if (!InputManager.Instance.RightActionHeld() && !_casting)
        {
            stateManager.SwitchState(stateManager.playerFishingState);
            return;
        }

        if(InputManager.Instance.RightActionRelease() && _casting)
        {
            //results here is the cast multiplier
            float results = _maxBarFill * _castBar.GetMultiplier();
            _castBar.UnfillCastMeter(false);

            //hard coded 15 will be replaced with equipment rating
            stateManager.bobber.Cast(stateManager.transform, results * 15);
            return;
        }

        if(InputManager.Instance.LeftAction() && !_casting)
        {
            _castBar.FillCastMeter(true);
            _casting = true;
        }

        if(InputManager.Instance.LeftActionRelease() && _casting)
        {
            _castBar.FillCastMeter(false);
            _maxBarFill = _castBar.barFill;

            //These values are hard coded for testing but, will be determined from equipment in later builds
            _castBar.AssignSuccessVars(0.1f, 0.3f, 0.3f);

            _castBar.UnfillCastMeter(true);
        }
    }

    private void ExitCastSuccessfully(bool state)
    {
        _casting = false;

        if (state)
            _stateManager.SwitchState(_stateManager.playerHookState);
        else
            _stateManager.SwitchState(_stateManager.playerFishingState);
    }
}
