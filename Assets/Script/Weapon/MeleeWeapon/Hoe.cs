using UnityEngine;

public class Hoe : IEnemyWeapon
{
    private Animator m_Animator;
    private GameObject Effect;
    private bool isAttackHit;
    public Hoe(GameObject obj, ICharacter character) : base(obj, character)
    {
        Effect = gameObject.transform.Find("Effect").gameObject;
        m_Attr = AttributeFactory.Instance.GetEnemyWeaponAttr(EnemyWeaponType.Hoe);
        m_Animator = gameObject.GetComponent<Animator>();
        TriggerCenter.Instance.RegisterObserver(TriggerType.OnTriggerEnter, Effect, "Player", HitPlayer);
    }
    protected override void OnFire()
    {
        base.OnFire();
        isAttackHit = false;
        m_Animator.SetBool("isAttack", true);
        CoroutinePool.Instance.StartAnimatorCallback(m_Animator, "Attack", () =>
        {
            m_Animator.SetBool("isAttack", false);
        }, this);

    }
    private void HitPlayer(GameObject obj)
    {
        if (!isAttackHit)
        {
            isAttackHit = true;
            (obj.transform.GetComponent<Symbol>().GetCharacter() as IPlayer).UnderAttack((m_Character.m_Attr as EnemyAttribute).m_ShareAttr.Damage);
        }
    }
}
