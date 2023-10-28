using UnityEngine;

public class DireBoarRunState : EnemyState
{
    private float Timer;
    private Animator m_EffectAnim;
    private SpriteRenderer m_Render;
    private Vector2 TargetPos;
    public DireBoarRunState(EnemyStateController controller) : base(controller)
    {
        TriggerCenter.Instance.RegisterObserver(TriggerType.OnTriggerEnter, m_HitPlayerBox, "Player", OnHitPlayer);
        m_EffectAnim = m_HitPlayerBox.GetComponent<Animator>();
        m_Render = m_HitPlayerBox.GetComponent<SpriteRenderer>();
    }
    protected override void StateStart()
    {
        base.StateStart();
        m_HitPlayerBox.SetActive(true);
        m_Attr.isAttack = true;
        m_Animator.SetBool("isIdle", false);
        Timer = 0;
        StartSeekerLoop();
    }
    public override void GameUpdate()
    {
        base.GameUpdate();
        switch (m_State)
        {
            case EnemyCondition.Roaming:
                if (IsInAttackDistance())
                {
                    m_State = EnemyCondition.Attack;
                }
                m_Attr.isAttack = false;
                if (isReachTarget)
                {
                    TargetPos = GetRamdomPositionAroundEnemy(6);
                }
                MoveToTarget(TargetPos);
                m_EffectAnim.enabled = false;
                m_Render.enabled = false;
                break;

            case EnemyCondition.Attack:
                if (!IsInAttackDistance())
                {
                    m_State = EnemyCondition.Roaming;
                }
                m_Attr.isAttack = true;
                m_EffectAnim.enabled = true;
                m_Render.enabled = true;
                if (isReachTarget)
                {
                    TargetPos = player.transform.position;
                }
                MoveToTarget(TargetPos);
                break;
        }
        if (Timer > 5)
        {
            m_Controller.SetOtherState(typeof(DireBoarIdleState));
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
        Debug.Log(1);
        if (m_Attr.isAttack)
        {
            IPlayer player = obj.GetComponent<Symbol>().GetCharacter() as IPlayer;
            if (player.m_Attr.HurtInvincibleTimer > player.m_Attr.m_ShareAttr.HurtInvincibleTime)
            {
                player.UnderEnemyAttack(m_Attr);
            }
        }
    }
}
