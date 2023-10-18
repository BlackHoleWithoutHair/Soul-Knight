using UnityEngine;

public class StakeBeAttackState : EnemyState
{
    private AnimatorStateInfo info;
    public StakeBeAttackState(EnemyStateController controller) : base(controller)
    {

    }
    public override void GameStart()
    {
        base.GameStart();
        m_Animator.SetBool("isAttack", true);
    }
    public override void GameUpdate()
    {
        base.GameUpdate();
        info = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("BeAttack") && info.normalizedTime >= 1)
        {
            m_Controller.SetOtherState(typeof(StakeIdleState));
        }
    }
}
