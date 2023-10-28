using System.Collections;
using UnityEngine;

public class TrumpetFlowerIdleState : EnemyState
{
    public TrumpetFlowerIdleState(EnemyStateController controller) : base(controller) { }
    protected override void StateStart()
    {
        base.StateStart();
        m_Animator.SetBool("isIdle", true);
        CoroutinePool.Instance.StartCoroutine(WaitForAttack(), this);
    }
    protected override void StateEnd()
    {
        base.StateEnd();
        CoroutinePool.Instance.StopAllCoroutineInObject(this);
    }
    private IEnumerator WaitForAttack()
    {
        yield return new WaitForSeconds(5.5f);
        m_Controller.SetOtherState(typeof(TrumpetFlowerAttackState));
    }
}
