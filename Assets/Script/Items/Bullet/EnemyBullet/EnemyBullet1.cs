using UnityEngine;

public class EnemyBullet1 : IEnemyBullet
{
    public EnemyBullet1(GameObject obj, EnemyWeaponShareAttribute attr) : base(obj, attr)
    {
        type = EnemyBulletType.EnemyBullet1;
    }
}
