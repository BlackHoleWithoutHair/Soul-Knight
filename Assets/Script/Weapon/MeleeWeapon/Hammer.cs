using System.Collections;
using UnityEngine;

public class Hammer : IEnemyWeapon
{
    private Animator m_Animator;
    public Hammer(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetEnemyWeaponAttr(EnemyWeaponType.Hammer);
        m_Animator = gameObject.GetComponent<Animator>();
    }
    protected override void OnFire()
    {
        base.OnFire();
        m_Animator.enabled = true;
        m_Animator.Play("Attack", 0, 0);
        CoroutinePool.Instance.StartCoroutine(Shot(), this);
    }
    private IEnumerator Shot()
    {
        yield return new WaitForSeconds(1f / 12f);
        for (float angle = 0; angle <= 360; angle += 72)
        {
            for (int i = -3; i <= 3; i++)
            {
                float a = angle + i * 30f / 7f;
                IEnemyBullet bullet = EffectFactory.Instance.GetEnemyBullet(EnemyBulletType.EnemyBullet1, m_Attr, m_Character.m_Attr.GetShareAttr() as EnemyShareAttr, FirePoint.transform.position, Quaternion.Euler(0, 0, a)) as IEnemyBullet;
                bullet.AddToController();
            }
        }
    }
}
