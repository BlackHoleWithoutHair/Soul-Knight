using System.Collections;
using UnityEngine;

public class PetIdleState : PetState
{
    public PetIdleState(PetStateController controller) : base(controller) { }
    protected override void StateStart()
    {
        base.StateStart();
        m_Animator.SetBool("isWalk", false);
        m_rb.velocity = Vector2.zero;
        if (m_Controller.target is IEnemy)
        {
            CoroutinePool.Instance.StartCoroutine(AttackLoop(), this);
        }
    }
    protected override void StateUpdate()
    {
        base.StateUpdate();
        if (GetDistanceToTarget(m_Controller.target.gameObject) > 5)
        {
            m_Controller.SetOtherState(typeof(PetFollowTargetState));
            return;
        }
        if (GetDistanceToTarget(player.gameObject) > 20)
        {
            m_Controller.SetOtherState(typeof(PetFollowTargetState));
            return;
        }
    }
    protected override void StateEnd()
    {
        base.StateEnd();
        CoroutinePool.Instance.StopAllCoroutineInObject(this);
    }
    private IEnumerator AttackLoop()
    {
        while (true)
        {
            if (m_Controller.target.IsDie)//打死一个敌人就会回到玩家身边
            {
                m_Controller.target = player;
                yield break;
            }
            m_Controller.target.UnderAttack(3);
            yield return new WaitForSeconds(2);
        }
    }
}
