using UnityEngine;

public class RangerIdleState : PlayerState
{
    public RangerIdleState(PlayerStateController controller) : base(controller)
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
            m_Controller.SetOtherState(typeof(RangerWalkState));
            return;
        }
        if (player.IsDie)
        {
            m_Controller.SetOtherState(typeof(RangerDieState));
            return;
        }
        player.GetUsedWeapon().OnUpdate();
        if (InputUtility.Instance.GetKeyDown(KeyAction.Skill)&&player.CanUseSkill())
        {
            if (player.m_Attr.CurrentSkillType == SkillType.Roll)
            {
                m_Controller.SetOtherState(typeof(RangerSkill1State));
                return;
            }
            else
            {
                m_Controller.SetOtherState(typeof(RangerSkill2State));
                return;
            }
        }
    }
    protected override void StateEnd()
    {
        base.StateEnd();
    }
}
