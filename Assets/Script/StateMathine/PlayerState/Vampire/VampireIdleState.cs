using UnityEngine;

public class VampireIdleState : PlayerState
{
    public VampireIdleState(PlayerStateController controller) : base(controller)
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
            m_Controller.SetOtherState(typeof(VampireWalkState));
            return;
        }
        if (player.IsDie)
        {
            m_Controller.SetOtherState(typeof(VampireDieState));
            return;
        }
        player.GetUsedWeapon().OnUpdate();
    }
    protected override void StateEnd()
    {
        base.StateEnd();
    }
}
