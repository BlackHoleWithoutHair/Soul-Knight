using UnityEngine;

public class EnemyRedArrow : IEnemyBullet
{
    public EnemyRedArrow(GameObject obj, EnemyWeaponShareAttribute attr) : base(obj, attr)
    {
        type = EnemyBulletType.EnemyRedArrow;
    }
}
