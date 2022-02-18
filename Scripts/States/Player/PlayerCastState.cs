using UnityEngine;
public class PlayerCastState : PlayerBaseState
{
    private GameObject _castBarObj;
    private CastBar _castBar;
    private bool _casting = false;

    public override void EnterState(PlayerStateManager stateManager)
    {
        if(!_castBarObj)
        {
            _castBarObj = stateManager.fishingRod.transform.Find("CastBar").gameObject;
            _castBar = _castBarObj.GetComponent<CastBar>();
        }

        stateManager.SetPlayerControls(false);
        _castBarObj.SetActive(true);
        Cursor.visible = false;
        _castBar.ClearBar();
    }

    public override void ExitState(PlayerStateManager stateManager)
    {
        _castBarObj.SetActive(false);
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (InputManager.Instance.PressedEscape())
        {
            UIManager.Instance.LoadUIDocument(new PauseMenuDocumentLogic());
            stateManager.SwitchState(stateManager.playerMenuState);
            return;
        }

        if(!InputManager.Instance.LeftActionHeld() && !_casting)
        {
            stateManager.SwitchState(stateManager.playerFishingState);
            return;
        }


        if(!InputManager.Instance.LeftActionHeld() && _casting)
        {
            //results here is the cast multiplier
            _castBar.UnfillCastMeter(false);
            _casting = false;
            Debug.Log($"Results: {_castBar.GetResults()}");

            return;
        }

        if(InputManager.Instance.RightAction() && !_casting)
        {
            _castBar.FillCastMeter(true);
            _casting = true;
        }

        if(InputManager.Instance.RightActionRelease() && _casting)
        {
            //Max fill will determine how much max cast is available
            _castBar.FillCastMeter(false);
            Debug.Log($"Max fill: {_castBar.barFill}");

            //These values are hard coded for testing but, will be determined from equipment in later builds
            _castBar.AssignSuccessVars(0.1f, 0.3f, 0.3f);

            _castBar.UnfillCastMeter(true);
        }


    }
}
