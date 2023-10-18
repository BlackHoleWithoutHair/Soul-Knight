using System.Collections;
using UnityEngine;

public class DireBoarIdleState : EnemyState
{
    private bool isFirstEnter;
    public DireBoarIdleState(EnemyStateController controller) : base(controller)
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
            m_Controller.SetOtherState(typeof(DireBoarRunState));
        }
        else
        {
            CoroutinePool.Instance.StartCoroutine(AttackPlayer(), this);
        }
    }
    public override void GameUpdate()
    {
        base.GameUpdate();

    }
    public override void GameExit()
    {
        base.GameExit();
    }
    private IEnumerator AttackPlayer()
    {
        yield return new WaitForSeconds(2);
        m_Controller.SetOtherState(typeof(DireBoarRunState));
    }
}
