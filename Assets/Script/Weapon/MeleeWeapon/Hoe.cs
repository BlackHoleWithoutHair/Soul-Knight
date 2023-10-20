using UnityEngine;

public class Hoe : IEnemyWeapon
{
    private Animator m_Animator;
    private GameObject Effect;
    private AnimatorStateInfo info;
    private bool isAttackHit;
    private bool isFire;
    public Hoe(GameObject obj, ICharacter character) : base(obj, character)
    {
        Effect = m_GameObject.transform.Find("Effect").gameObject;
        m_Attr = AttributeFactory.Instance.GetEnemyWeaponAttr(EnemyWeaponType.Hoe);
        m_Animator = m_GameObject.GetComponent<Animator>();
        TriggerCenter.Instance.RegisterObserver(TriggerType.OnTriggerEnter, Effect, "Player", HitPlayer);
    }
    protected override void OnFire()
    {
        base.OnFire();
        isAttackHit = false;
        isFire = true;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (isFire)
        {
            m_Animator.SetBool("isAttack", true);
            info = m_Animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Attack") && info.normalizedTime > 1)
            {
                m_Animator.SetBool("isAttack", false);
                isFire = false;
            }
        }
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
