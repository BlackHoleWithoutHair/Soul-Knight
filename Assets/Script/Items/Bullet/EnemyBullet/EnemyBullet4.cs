using UnityEngine;

public class EnemyBullet4 : IEnemyBullet
{
    public EnemyBullet4(GameObject obj, EnemyWeaponShareAttribute attr) : base(obj, attr)
    {
        type = EnemyBulletType.EnemyBullet4;
    }
}
