using System.Collections;
using UnityEngine;

public class EliteGoblinGuardIdleState : EnemyState
{
    private bool isFirstEnter;
    public EliteGoblinGuardIdleState(EnemyStateController controller) : base(controller)
    {

    }
    protected override void StateStart()
    {
        base.StateStart();
        m_rb.velocity = Vector2.zero;
        m_Animator.SetBool("isIdle", true);
        if (!isFirstEnter)
        {
            isFirstEnter = true;
            m_Controller.SetOtherState(typeof(EliteGoblinGuardAttackState));
        }
        else
        {
            CoroutinePool.Instance.StartCoroutine(WaitForAttack(), this);
        }
    }
    protected override void StateEnd()
    {
        base.StateEnd();
        CoroutinePool.Instance.StopAllCoroutineInObject(this);
    }
    private IEnumerator WaitForAttack()
    {
        yield return new WaitForSeconds(3f);
        m_Controller.SetOtherState(typeof(EliteGoblinGuardAttackState));
    }
}
