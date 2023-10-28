
using UnityEngine;

public class BossState : IState
{
    protected IBoss boss;
    protected Animator m_Animator;
    public BossState(BossStateController controller) : base(controller) { }
    protected override void StateInit()
    {
        base.StateInit();
        boss = (m_Controller as BossStateController).m_Boss;
        m_Animator = boss.transform.GetComponent<Animator>();

    }
}
