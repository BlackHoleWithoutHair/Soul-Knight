using UnityEngine;

public class EnemyBullet3 : IEnemyBullet
{
    public EnemyBullet3(GameObject obj) : base(obj)
    {
        type = EnemyBulletType.EnemyBullet3;
    }
}
