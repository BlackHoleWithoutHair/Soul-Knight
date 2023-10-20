using UnityEngine;

public class BoarAttackState : EnemyState
{
    private float Timer;
    private float RandomTimer;
    private Vector2 TargetPos;
    public BoarAttackState(EnemyStateController controller) : base(controller)
    {
        TriggerCenter.Instance.RegisterObserver(TriggerType.OnTriggerEnter, m_HitPlayerBox, "Player", OnHitPlayer);
    }
    protected override void StateStart()
    {
        base.StateStart();
        m_State = EnemyCondition.Roaming;
        m_HitPlayerBox.SetActive(true);
        m_Attr.isAttack = true;
        m_Animator.SetBool("isIdle", false);
        Timer = 0;
        RandomTimer = 2;
        StartSeekerLoop();
    }
    public override void GameUpdate()
    {
        base.GameUpdate();
        Timer += Time.deltaTime;
        switch (m_State)
        {
            case EnemyCondition.Roaming:
                if (IsInAttackDistance())
                {
                    m_State = EnemyCondition.Attack;
                }
                RandomTimer += Time.deltaTime;
                if (RandomTimer > 2)
                {
                    RandomTimer = 0;
                    TargetPos = GetRamdomPositionAroundEnemy(5);
                }
                MoveToTarget(TargetPos);
                break;

            case EnemyCondition.Attack:
                if (!IsInAttackDistance())
                {
                    m_State = EnemyCondition.Roaming;
                }
                RandomTimer += Time.deltaTime;
                if (RandomTimer > 1)
                {
                    RandomTimer = 0;
                    TargetPos = player.transform.position;
                }
                MoveToTarget(TargetPos);
                break;
        }
        if (Timer > 7)
        {
            m_Controller.SetOtherState(typeof(BoarIdleState));
            return;
        }
    }
    protected override void StateEnd()
    {
        base.StateEnd();
        m_HitPlayerBox.SetActive(false);
        m_Attr.isAttack = false;
        StopSeekerLoop();
    }
    private void OnHitPlayer(GameObject obj)
    {
        IPlayer player = obj.GetComponent<Symbol>().GetCharacter() as IPlayer;
        if (player.m_Attr.HurtInvincibleTimer > player.m_Attr.m_ShareAttr.HurtInvincibleTime)
        {
            player.UnderEnemyAttack(m_Attr);
        }
    }
}
