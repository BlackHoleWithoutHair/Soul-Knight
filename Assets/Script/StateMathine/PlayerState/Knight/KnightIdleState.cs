using UnityEngine;

public class KnightIdleState : PlayerState
{
    public KnightIdleState(PlayerStateController controller) : base(controller) { }
    protected override void StateUpdate()
    {
        base.StateUpdate();
        m_rb.velocity = Vector2.zero;
        if (hor != 0 || ver != 0)
        {
            m_Controller.SetOtherState(typeof(KnightWalkState));
            return;
        }
        player.GetUsedWeapon().OnUpdate();
        if (player.IsDie)
        {
            m_Controller.SetOtherState(typeof(KnightDieState));
            return;
        }
    }
    protected override void StateEnd()
    {
        base.StateEnd();
    }
}
