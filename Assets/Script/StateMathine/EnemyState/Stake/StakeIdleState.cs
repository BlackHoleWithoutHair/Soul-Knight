public class StakeIdleState : EnemyState
{
    public StakeIdleState(EnemyStateController controller) : base(controller) { }
    protected override void StateStart()
    {
        base.StateStart();
        m_Animator.SetBool("isIdle", true);
    }
}
