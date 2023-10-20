using System.Collections;
using UnityEngine;

public class GoblinShamanIdleState : EnemyState
{
    private bool isFirstEnter;
    public GoblinShamanIdleState(EnemyStateController controller) : base(controller) { }
    protected override void StateStart()
    {
        base.StateStart();
        m_rb.velocity = Vector2.zero;
        m_Animator.SetBool("isIdle", true);
        if (!isFirstEnter)
        {
            isFirstEnter = true;
            m_Controller.SetOtherState(typeof(GoblinShamanAttackState));
        }
        else
        {
            CoroutinePool.Instance.StartCoroutine(WaitForAttack(), this);
        }
        CoroutinePool.Instance.StartCoroutine(WaitForAttack(), this);
    }
    private IEnumerator WaitForAttack()
    {
        yield return new WaitForSeconds(3f);
        m_Controller.SetOtherState(typeof(GoblinShamanAttackState));
    }
}
