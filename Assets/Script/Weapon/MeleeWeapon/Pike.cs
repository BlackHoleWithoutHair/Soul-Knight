using UnityEngine;

public class Pike : IEnemyWeapon
{
    private Animator m_Animator;
    private Animator m_EffectAnimator;
    private GameObject Effect;
    private bool isAttackHit;
    public Pike(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetEnemyWeaponAttr(EnemyWeaponType.Pike);
        m_Animator = UnityTool.Instance.GetComponentFromChild<Animator>(gameObject, "Pike");
        Effect = UnityTool.Instance.GetGameObjectFromChildren(gameObject, "Effect");
        m_EffectAnimator = Effect.GetComponent<Animator>();
        TriggerCenter.Instance.RegisterObserver(TriggerType.OnTriggerEnter, Effect, "Player", HitPlayer);
    }

    protected override void OnFire()
    {
        base.OnFire();
        m_Animator.SetBool("isAttack", true);
        isAttackHit = false;
        CoroutinePool.Instance.StartAnimatorCallback(m_Animator, "Attack", () =>
        {
            Effect.SetActive(true);
            m_EffectAnimator.Play("Sting", 0, 0);
        }, this);
        CoroutinePool.Instance.StartAnimatorCallback(m_EffectAnimator, "Sting", () =>
        {
            Effect.SetActive(false);
            m_Animator.SetBool("isAttack", false);
        }, this);
    }
    private void HitPlayer(GameObject obj)
    {
        if (!isAttackHit)
        {
            isAttackHit = true;
            (obj.transform.parent.GetComponent<Symbol>().GetCharacter() as IPlayer).UnderAttack((m_Character.m_Attr as EnemyAttribute).m_ShareAttr.Damage);
            Debug.Log(obj.transform.parent);
            Debug.Log(m_Character.m_Attr);
        }
    }
}
