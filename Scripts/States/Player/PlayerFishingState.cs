using UnityEngine;
public class PlayerFishingState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager stateManager)
    {
        stateManager.fishingRod.SetActive(true);
        stateManager.bobber.SetActive(true);
        stateManager.SetPlayerControls(true);
    }

    public override void ExitState(PlayerStateManager stateManager)
    {
        
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if(InputManager.Instance.ToggledRod())
        {
            stateManager.fishingRod.SetActive(false);
            stateManager.bobber.SetActive(false);
            stateManager.SwitchState(stateManager.playerRoamState);
            return;
        }

        if(InputManager.Instance.LeftAction())
        {
            stateManager.SwitchState(stateManager.playerCastState);
            return;
        }
    }
}
