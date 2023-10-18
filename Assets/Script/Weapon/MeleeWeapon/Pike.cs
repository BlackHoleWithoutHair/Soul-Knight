using UnityEngine;

public class Pike : IEnemyWeapon
{
    private Animator m_Animator;
    private Animator m_EffectAnimator;
    private GameObject Effect;
    private AnimatorStateInfo info;
    private AnimatorStateInfo EffectInfo;
    private bool isAttackHit;
    private bool isFire;
    public Pike(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetEnemyWeaponAttr(EnemyWeaponType.Pike);
        m_Animator = UnityTool.Instance.GetComponentFromChild<Animator>(m_GameObject, "Pike");
        Effect = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "Effect");
        m_EffectAnimator = Effect.GetComponent<Animator>();
        TriggerCenter.Instance.RegisterObserver(TriggerType.OnTriggerEnter, Effect, "Player", HitPlayer);
    }
    protected override void OnFire()
    {
        base.OnFire();
        m_Animator.SetBool("isAttack", true);
        isAttackHit = false;
        isFire = true;
    }
    public override void OnUpdate()
    {
        if (isFire)
        {
            info = m_Animator.GetCurrentAnimatorStateInfo(0);
            EffectInfo = m_EffectAnimator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Attack") && info.normalizedTime > 1 && !Effect.gameObject.activeSelf)
            {
                Effect.SetActive(true);
                m_EffectAnimator.Play("Sting", 0, 0);
            }
            if (Effect.activeSelf)
            {
                if (EffectInfo.IsName("Sting") && EffectInfo.normalizedTime > 1)
                {
                    Effect.SetActive(false);
                    m_Animator.SetBool("isAttack", false);
                    isFire = false;
                }
            }
        }
    }
    private void HitPlayer(GameObject obj)
    {
        if (!isAttackHit)
        {
            isAttackHit = true;
            (obj.transform.parent.GetComponent<Symbol>().GetCharacter() as IPlayer).UnderWeaponAttack(m_Attr);
        }
    }
}
