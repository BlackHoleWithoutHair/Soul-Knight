using UnityEngine;

public class EnemyBullet5 : IEnemyBullet
{
    private float Timer;
    public EnemyBullet5(GameObject obj, EnemyWeaponShareAttribute attr) : base(obj, attr)
    {
        type = EnemyBulletType.EnemyBullet5;
    }
    protected override void BeforeHitWallUpdate()
    {
        base.BeforeHitWallUpdate();
        Timer += Time.deltaTime;
        if (Timer > 1)
        {
            Damage = 2;
            EnemyWeaponShareAttribute attr = new EnemyWeaponShareAttribute();
            attr.Speed = 5;
            attr.DebuffType = BuffType.None;
            for (int i = -1; i <= 1; i++)
            {
                IBullet bullet = EffectFactory.Instance.GetEnemyBullet(EnemyBulletType.EnemyBullet1, attr, gameObject.transform.position, Quaternion.Euler(0, 0, i * 15) * m_Rot);
                (bullet as IEnemyBullet).SetDamage(Damage);
                bullet.AddToController();
            }
            Remove();
        }
    }
}
