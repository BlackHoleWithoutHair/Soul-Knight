public class StakeIdleState : EnemyState
{
    public StakeIdleState(EnemyStateController controller) : base(controller)
    {

    }
    public override void GameStart()
    {
        base.GameStart();
        m_Animator.SetBool("isAttack", false);

    }

}
