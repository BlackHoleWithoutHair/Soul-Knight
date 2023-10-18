using System.Collections;
using UnityEngine;

public class EliteGoblinGuardMeleeAttackState : EnemyState
{
    //the timer start when enter this state
    private float Timer;
    private Vector2 TargetPos;
    public EliteGoblinGuardMeleeAttackState(EnemyStateController controller) : base(controller)
    {
        //TriggerSystem.Instance.RegisterObserver(TriggerType.OnTriggerEnter, m_Effect, player.gameObject, OnHitPlayer);
    }

    public override void GameStart()
    {
        base.GameStart();
        m_Attr.isAttack = true;
        m_Animator.SetBool("isWalk", true);
        Timer = 0;
        StartSeekerLoop();
    }
    public override void GameUpdate()
    {
        base.GameUpdate();
        Timer += Time.deltaTime;
        switch (m_State)
        {
            case EnemyCondition.Roaming:
                if (IsFindPlayer())
                {
                    m_State = EnemyCondition.ChaseTarget;
                }
                if(isReachTarget)
                {
                    TargetPos = GetRamdomPositionAroundEnemy(5);
                }
                MoveToTarget(TargetPos);
                break;

            case EnemyCondition.ChaseTarget:
                if (!IsFindPlayer())
                {
                    m_State = EnemyCondition.Roaming;
                }
                if (IsInAttackDistance())
                {
                    m_State = EnemyCondition.Attack;
                }
                if(isReachTarget)
                {
                    TargetPos = player.transform.position;
                }
                MoveToTarget(TargetPos);
                break;

            case EnemyCondition.Attack:

                m_State = EnemyCondition.Roaming;
                CoroutinePool.Instance.StartCoroutine(Warning(0.4f, () =>
                {
                    CoroutinePool.Instance.StartCoroutine(UseWeapon());
                }), this);
                break;
        }
        if (Timer > 6)
        {
            m_Controller.SetOtherState(typeof(EliteGoblinGuardIdleState));
            return;
        }
    }
    public override void GameExit()
    {
        base.GameExit();
        m_Attr.isAttack = false;
        StopSeekerLoop();
    }
    private IEnumerator UseWeapon()
    {
        m_Character.m_Weapon.Fire();
        yield return null;
    }
}
