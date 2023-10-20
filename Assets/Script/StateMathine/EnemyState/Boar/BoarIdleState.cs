using System.Collections;
using UnityEngine;

public class BoarIdleState : EnemyState
{
    private bool isFirstEnter;
    public BoarIdleState(EnemyStateController controller) : base(controller)
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
            m_Controller.SetOtherState(typeof(BoarAttackState));
        }
        else
        {
            CoroutinePool.Instance.StartCoroutine(AttackPlayer(), this);
        }
    }
    private IEnumerator AttackPlayer()
    {
        yield return new WaitForSeconds(3);
        m_Controller.SetOtherState(typeof(BoarAttackState));
    }
}
