using System.Collections;
using UnityEngine;

public class TrumpetFlowerIdleState : EnemyState
{
    public TrumpetFlowerIdleState(EnemyStateController controller) : base(controller)
    {

    }
    public override void GameStart()
    {
        base.GameStart();
        m_Animator.SetBool("isAttack", false);
        CoroutinePool.Instance.StartCoroutine(WaitForAttack(), this);
    }
    public override void GameUpdate()
    {
        base.GameUpdate();
    }
    public override void GameExit()
    {
        base.GameExit();
        CoroutinePool.Instance.StopAllCoroutineInObject(this);
    }
    private IEnumerator WaitForAttack()
    {
        yield return new WaitForSeconds(Random.Range(4, 7));
        m_Controller.SetOtherState(typeof(TrumpetFlowerAttackState));
    }
}
