using UnityEngine;

public class EnemyBullet3 : IEnemyBullet
{
    public EnemyBullet3(GameObject obj, EnemyWeaponShareAttribute attr) : base(obj, attr)
    {
        type = EnemyBulletType.EnemyBullet3;
    }
}
