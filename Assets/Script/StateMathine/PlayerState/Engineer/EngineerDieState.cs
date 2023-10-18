using UnityEngine;

public class EngineerDieState : PlayerState
{
    public EngineerDieState(PlayerStateController controller) : base(controller)
    {

    }
    protected override void StateStart()
    {
        base.StateStart();
        m_Animator.SetBool("isDie", true);
        player.GetUsedWeapon().gameObject.SetActive(false);
    }
    protected override void StateUpdate()
    {
        base.StateUpdate();
        m_rb.velocity = Vector2.zero;
        if (!player.IsDie)
        {
            m_Controller.SetOtherState(typeof(EngineerIdleState));
        }
    }
    protected override void StateEnd()
    {
        base.StateEnd();
        m_Animator.SetBool("isDie", false);
        player.GetUsedWeapon().gameObject.SetActive(true);
    }
}
