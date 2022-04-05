using UnityEngine;
public class PlayerReelState : PlayerBaseState
{
    private PlayerStateManager _stateManager;
    private RodControls _rodControls;
    private FishBase _hookedFish;
    private Transform _hook;

    public override void EnterState(PlayerStateManager stateManager)
    {
        if (!_stateManager)
            _stateManager = stateManager;
        if (!_rodControls)
            _rodControls = stateManager.fishingRod.GetComponent<RodControls>();
        if (!_hook)
            _hook = stateManager.bobber.transform.Find("hook");

        Debug.Log("Hooked Fish!");
        _hookedFish = stateManager.currentFish;
        BuildFish();

        _rodControls.reelIn += ReelIn;
    }

    public override void ExitState(PlayerStateManager stateManager)
    {
        _rodControls.reelIn -= ReelIn;
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

    private void BuildFish()
    {
        GameObject newFish = new GameObject(_hookedFish.name);
        Fish fishScript = newFish.AddComponent<Fish>();
        fishScript.rodControls = _rodControls;
        fishScript.BuildFish(_hookedFish, _hook.position);
        _rodControls.hookedFish = fishScript;
        newFish.transform.parent = _hook;
        newFish.transform.position -= new Vector3(0f, newFish.transform.localScale.y / 2f, 0f);
        newFish.transform.rotation.eulerAngles.Set(270f, 0f, 0f);
    }

    private void ReelIn(bool fishOn)
    {
        _rodControls.enabled = false;

        if (fishOn)
        {
            //go to unhook
            _stateManager.SwitchState(_stateManager.playerFishingState);
        }
        else
        {
            _stateManager.SwitchState(_stateManager.playerFishingState);
        }
    }
}
