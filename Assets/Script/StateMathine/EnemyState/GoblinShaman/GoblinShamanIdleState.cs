using System.Collections;
using UnityEngine;

public class GoblinShamanIdleState : EnemyState
{
    private bool isFirstEnter;
    public GoblinShamanIdleState(EnemyStateController controller) : base(controller)
    {

    }
    public override void GameStart()
    {
        base.GameStart();
        m_rb.velocity = Vector2.zero;
        m_Animator.SetBool("isWalk", false);
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
        yield return new WaitForSeconds(3f);
        m_Controller.SetOtherState(typeof(GoblinShamanAttackState));
    }
}
