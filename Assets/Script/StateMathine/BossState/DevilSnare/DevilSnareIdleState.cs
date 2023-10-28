using System.Collections;
using UnityEngine;

public class DevilSnareIdleState : BossState
{
    private bool isEnterAttackState;
    private float AttackFrequency = 1;
    public DevilSnareIdleState(BossStateController controller) : base(controller) { }
    protected override void StateStart()
    {
        base.StateStart();
        CoroutinePool.Instance.StartCoroutine(WaitForAttack(), this);
        if (isEnterAttackState)
        {
            m_Animator.SetBool("isAngerIdle", true);
        }
        else
        {
            m_Animator.SetBool("isIdle", true);
        }
    }
    protected override void StateUpdate()
    {
        base.StateUpdate();
        if (boss.m_Attr.CurrentHp < boss.m_Attr.m_ShareAttr.MaxHp / 2 && !isEnterAttackState)
        {
            isEnterAttackState = true;
            AttackFrequency = 2;
            m_Animator.SetBool("isIdle", false);
            m_Animator.SetBool("isAngerIdle", true);
        }
    }
    protected override void StateEnd()
    {
        base.StateEnd();
        m_Animator.SetBool("isIdle", false);
        m_Animator.SetBool("isAngerIdle", false);
    }
    private IEnumerator WaitForAttack()
    {
        yield return new WaitForSeconds(Random.Range(2f, 5f) / AttackFrequency);
        m_Controller.SetOtherState(typeof(DevilSnareAttackState));
    }
}
