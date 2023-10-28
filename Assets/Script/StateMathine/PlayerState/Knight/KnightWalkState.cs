using UnityEngine;

public class KnightWalkState : PlayerState
{
    private Vector2 dir;
    public KnightWalkState(PlayerStateController controller) : base(controller) { }
    protected override void StateStart()
    {
        base.StateStart();
        m_Animator.SetBool("isWalk", true);
    }
    protected override void StateUpdate()
    {
        base.StateUpdate();
        m_Animator.SetBool("isWalk", true);
        dir.Set(hor, ver);
        if (ver == 0 && hor == 0)
        {
            m_Controller.SetOtherState(typeof(KnightIdleState));
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
        Movement(m_Attr.m_ShareAttr.Speed * dir.normalized);
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
        m_Animator.SetBool("isWalk", false);
    }
}
