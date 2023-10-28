using UnityEngine;

public class EnemyBullet1 : IEnemyBullet
{
    public EnemyBullet1(GameObject obj) : base(obj)
    {
        type = EnemyBulletType.EnemyBullet1;
    }
}
