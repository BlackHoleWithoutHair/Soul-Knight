using UnityEngine;

public class RangerWalkState : PlayerState
{
    private Vector2 dir;
    public RangerWalkState(PlayerStateController controller) : base(controller) { }
    protected override void StateUpdate()
    {
        base.StateUpdate();
        m_Animator.SetBool("isWalk", true);
        dir.Set(hor, ver);
        if (ver == 0 && hor == 0)
        {
            m_Controller.SetOtherState(typeof(RangerIdleState));
            return;
        }
        if (player.isLeft)
        {
            if (hor > 0)
            {
                m_Animator.SetFloat("Speed", -1);
            }
            else
            {
                m_Animator.SetFloat("Speed", 1);
            }
        }
        else
        {
            if (hor >= 0)
            {
                m_Animator.SetFloat("Speed", 1);
            }
            else
            {
                m_Animator.SetFloat("Speed", -1);
            }
        }
        m_rb.velocity = m_Attr.m_ShareAttr.Speed * dir.normalized;
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
        m_Animator.SetBool("isWalk", false);
    }
}
