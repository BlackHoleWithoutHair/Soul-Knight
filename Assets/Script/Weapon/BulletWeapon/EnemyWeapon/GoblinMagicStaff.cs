using UnityEngine;

public class GoblinMagicStaff : IEnemyWeapon
{
    private Animator m_Animator;
    private AnimatorStateInfo info;
    private bool isFire;
    public GoblinMagicStaff(GameObject obj, ICharacter enemy) : base(obj, enemy)
    {
        m_Attr = AttributeFactory.Instance.GetEnemyWeaponAttr(EnemyWeaponType.GoblinMagicStaff);
        m_Animator = m_GameObject.GetComponent<Animator>();
        CanBeRotated = false;
    }
    protected override void OnFire()
    {
        base.OnFire();
        m_Animator.enabled = true;
        m_Animator.Play("Attack", 0, 0);
        isFire = true;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (isFire)
        {
            info = m_Animator.GetCurrentAnimatorStateInfo(0);
            if (info.normalizedTime > 0.5f)
            {
                isFire = false;
                for (int i = -1; i <= 1; i++)
                {
                    IBullet bullet = EffectFactory.Instance.GetEnemyBullet(EnemyBulletType.EnemyBullet5, m_Attr, FirePoint.transform.position, Quaternion.Euler(0, 0, i * 15) * GetShotRotation());
                    (bullet as IEnemyBullet).SetDamage((m_Character.m_Attr as EnemyAttribute).m_ShareAttr.Damage);
                    bullet.AddToController();
                }
            }
        }
    }
}
