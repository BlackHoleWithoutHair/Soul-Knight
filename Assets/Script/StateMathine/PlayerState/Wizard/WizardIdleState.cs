using UnityEngine;

public class WizardIdleState : PlayerState
{
    public WizardIdleState(PlayerStateController controller) : base(controller)
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
            m_Controller.SetOtherState(typeof(WizardWalkState));
            return;
        }
        if (player.IsDie)
        {
            m_Controller.SetOtherState(typeof(WizardDieState));
            return;
        }
        player.GetUsedWeapon().OnUpdate();
    }
    protected override void StateEnd()
    {
        base.StateEnd();
    }
}
