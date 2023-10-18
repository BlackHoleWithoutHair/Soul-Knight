using UnityEngine;

public class WizardWalkState : PlayerState
{
    private Vector2 dir;
    public WizardWalkState(PlayerStateController controller) : base(controller)
    {

    }
    protected override void StateStart()
    {
        base.StateStart();
    }
    protected override void StateUpdate()
    {
        base.StateUpdate();
        m_Animator.SetBool("isWalk", true);
        dir.Set(hor, ver);
        if (ver == 0 && hor == 0)
        {
            m_Controller.SetOtherState(typeof(WizardIdleState));
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
    }
    protected override void StateEnd()
    {
        base.StateEnd();
        m_Animator.SetBool("isWalk", false);
    }
}
