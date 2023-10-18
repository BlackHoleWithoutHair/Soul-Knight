using UnityEngine;

public class VampireDieState : PlayerState
{
    public VampireDieState(PlayerStateController controller) : base(controller)
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
            m_Controller.SetOtherState(typeof(VampireIdleState));
        }
    }
    protected override void StateEnd()
    {
        base.StateEnd();
        m_Animator.SetBool("isDie", false);
        player.GetUsedWeapon().gameObject.SetActive(true);
    }
}
