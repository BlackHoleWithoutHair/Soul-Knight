using UnityEngine;

public class EnemyRedArrow : IEnemyBullet
{
    public EnemyRedArrow(GameObject obj) : base(obj)
    {
        type = EnemyBulletType.EnemyRedArrow;
    }
}
