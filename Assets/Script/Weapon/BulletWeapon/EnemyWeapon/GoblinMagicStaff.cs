using System.Collections;
using UnityEngine;

public class GoblinMagicStaff : IEnemyWeapon
{
    private Animator m_Animator;
    private AnimatorStateInfo info;
    public GoblinMagicStaff(GameObject obj, ICharacter enemy) : base(obj, enemy)
    {
        m_Attr = AttributeFactory.Instance.GetEnemyWeaponAttr(EnemyWeaponType.GoblinMagicStaff);
        m_Animator = gameObject.GetComponent<Animator>();
        CanBeRotated = false;
    }
    protected override void OnFire()
    {
        base.OnFire();
        m_Animator.enabled = true;
        m_Animator.Play("Attack", 0, 0);
        info = m_Animator.GetCurrentAnimatorStateInfo(0);
        CoroutinePool.Instance.StartCoroutine(WaitForFire(), this);
    }
    private IEnumerator WaitForFire()
    {
        yield return info.length * 0.5f;
        for (int i = -1; i <= 1; i++)
        {
            IBullet bullet = EffectFactory.Instance.GetEnemyBullet(EnemyBulletType.EnemyBullet5, m_Attr, m_Character.m_Attr.GetShareAttr() as EnemyShareAttr, FirePoint.transform.position, Quaternion.Euler(0, 0, i * 15) * GetShotRotation());
            bullet.AddToController();
        }
    }
}
