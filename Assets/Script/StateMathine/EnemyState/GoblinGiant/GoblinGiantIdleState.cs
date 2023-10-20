using System.Collections;
using UnityEngine;

public class GoblinGiantIdleState : EnemyState
{
    private bool isFirstEnter;
    public GoblinGiantIdleState(EnemyStateController controller) : base(controller) { }
    protected override void StateStart()
    {
        base.StateStart();
        m_rb.velocity = Vector2.zero;
        m_Animator.SetBool("isIdle", true);
        if (!isFirstEnter)
        {
            isFirstEnter = true;
            m_Controller.SetOtherState(typeof(GoblinGiantAttackState));
        }
        else
        {
            CoroutinePool.Instance.StartCoroutine(AttackPlayer(), this);
        }
    }
    private IEnumerator AttackPlayer()
    {
        yield return new WaitForSeconds(2);
        m_Controller.SetOtherState(typeof(GoblinGiantAttackState));
    }
}
