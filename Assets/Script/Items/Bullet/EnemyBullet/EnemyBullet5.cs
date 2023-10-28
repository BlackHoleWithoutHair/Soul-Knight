using UnityEngine;

public class EnemyBullet5 : IEnemyBullet
{
    private float Timer;
    public EnemyBullet5(GameObject obj) : base(obj)
    {
        type = EnemyBulletType.EnemyBullet5;
    }
    protected override void BeforeHitObstacleUpdate()
    {
        base.BeforeHitObstacleUpdate();
        Timer += Time.deltaTime;
        if (Timer > 1)
        {
            EnemyBulletShareAttr attr = new EnemyBulletShareAttr();
            attr.Speed = 5;
            attr.DebuffType = BuffType.None;
            attr.Damage = 2;
            for (int i = -1; i <= 1; i++)
            {
                IBullet bullet = EffectFactory.Instance.GetEnemyBullet(EnemyBulletType.EnemyBullet1, attr, gameObject.transform.position, Quaternion.Euler(0, 0, i * 15) * m_Rot);
                bullet.AddToController();
            }
            Remove();
        }
    }
}
