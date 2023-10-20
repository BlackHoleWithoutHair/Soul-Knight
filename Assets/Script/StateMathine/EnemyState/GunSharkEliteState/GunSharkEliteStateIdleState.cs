using System.Collections;
using UnityEngine;

public class GunSharkEliteStateIdleState : EnemyState
{
    private bool isFirstEnter;
    public GunSharkEliteStateIdleState(EnemyStateController controller) : base(controller) { }
    protected override void StateStart()
    {
        base.StateStart();
        m_rb.velocity = Vector2.zero;
        m_Animator.SetBool("isIdle", true);
        if (!isFirstEnter)
        {
            isFirstEnter = true;
            m_Controller.SetOtherState(typeof(GunSharkEliteStateRunState));
        }
        else
        {
            CoroutinePool.Instance.StartCoroutine(AttackPlayer(), this);
        }
    }
    private IEnumerator AttackPlayer()
    {
        yield return new WaitForSeconds(2);
        m_Controller.SetOtherState(typeof(GunSharkEliteStateRunState));
    }
}
