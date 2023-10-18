using UnityEngine;

public class WerewolfIdleState : PlayerState
{
    public WerewolfIdleState(PlayerStateController controller) : base(controller)
    {

    }
    protected override void StateStart()
    {
        base.StateStart();
    }
    protected override void StateUpdate()
    {
        base.StateUpdate();
        m_rb.velocity = Vector2.zero;
        if (hor != 0 || ver != 0)
        {
            m_Controller.SetOtherState(typeof(WerewolfWalkState));
            return;
        }
        if (player.IsDie)
        {
            m_Controller.SetOtherState(typeof(WerewolfDieState));
            return;
        }
        player.GetUsedWeapon().OnUpdate();
    }
    protected override void StateEnd()
    {
        base.StateEnd();
    }
}
