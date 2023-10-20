using System.Collections;
using System.Net.Sockets;
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
        yield return new WaitForSeconds(Random.Range(4, 7));
        m_Controller.SetOtherState(typeof(TrumpetFlowerAttackState));
    }
}
