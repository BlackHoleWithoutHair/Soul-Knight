using UnityEngine;

public class KnightIdleState : PlayerState
{
    public KnightIdleState(PlayerStateController controller) : base(controller)
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
            m_Controller.SetOtherState(typeof(KnightWalkState));
            return;
        }
        player.GetUsedWeapon().OnUpdate();
        if (player.IsDie)
        {
            m_Controller.SetOtherState(typeof(KnightDieState));
            return;
        }
        if (InputUtility.Instance.GetKeyDown(KeyAction.Skill))
        {
            player.Skill.StartSkill();
        }
    }
    protected override void StateEnd()
    {
        base.StateEnd();
    }
}
