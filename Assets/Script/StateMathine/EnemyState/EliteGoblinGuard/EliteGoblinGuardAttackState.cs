using System.Collections;
using UnityEngine;

public class EliteGoblinGuardAttackState : EnemyState
{
    //the timer start when enter this state
    private float Timer;
    //the timer start when enemy roaming
    private float RoamingTimer;
    private Vector2 TargetPos;
    public EliteGoblinGuardAttackState(EnemyStateController controller) : base(controller) { }
    protected override void StateStart()
    {
        base.StateStart();
        m_State = EnemyCondition.Roaming;
        m_Attr.isAttack = true;
        m_Animator.SetBool("isIdle", false);
        RoamingTimer = 1.5f;
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
                RoamingTimer += Time.deltaTime;
                if (RoamingTimer > Random.Range(3, 5))
                {
                    RoamingTimer = 0;
                    m_State = EnemyCondition.Attack;
                }
                if (isReachTarget)
                {
                    TargetPos = GetRamdomPositionAroundEnemy(5);
                }
                MoveToTarget(TargetPos);
                break;

            case EnemyCondition.Attack:
                m_State = EnemyCondition.Roaming;
                m_Character.m_Weapon.StartBeforeFireUpdate();
                CoroutinePool.Instance.StartCoroutine(Warning(0.4f, () =>
                {
                    CoroutinePool.Instance.StartCoroutine(UseWeapon());
                }), this);

                break;
        }
        if (Timer > 7)
        {
            m_Controller.SetOtherState(typeof(EliteGoblinGuardIdleState));
            return;
        }
    }
    protected override void StateEnd()
    {
        base.StateEnd();
        m_Attr.isAttack = false;
        StopSeekerLoop();
    }
    private IEnumerator UseWeapon()
    {
        m_Character.m_Weapon.Fire();
        yield return null;
    }
}
