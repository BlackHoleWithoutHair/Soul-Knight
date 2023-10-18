using System.Collections;
using UnityEngine;

public class BoarIdleState : EnemyState
{
    private bool isFirstEnter;
    public BoarIdleState(EnemyStateController controller) : base(controller)
    {

    }
    public override void GameStart()
    {
        base.GameStart();
        m_rb.velocity = Vector2.zero;
        m_Animator.SetBool("isRun", false);
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
