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
            //Get results
        }

        if(InputManager.Instance.RightAction() && !_casting)
        {
            //Start to fill bar
            _casting = true;
        }

        if(!InputManager.Instance.RightActionHeld() && _casting)
        {
            //Stop filling bar
            //Get results
        }


    }
}
