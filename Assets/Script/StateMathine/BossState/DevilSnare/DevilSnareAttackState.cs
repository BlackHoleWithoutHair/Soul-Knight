using System.Collections.Generic;
using UnityEngine;
public class DevilSnareAttackState : BossState
{
    private List<IBossSkill> skills;
    public DevilSnareAttackState(BossStateController controller) : base(controller) { }
    protected override void StateInit()
    {
        base.StateInit();
        skills = boss.skills;
    }
    protected override void StateStart()
    {
        base.StateStart();
        m_Animator.SetTrigger("isAttack");
        skills[Random.Range(0, skills.Count)].StartSkill();
        m_Controller.SetOtherState(typeof(DevilSnareIdleState));
    }
}
