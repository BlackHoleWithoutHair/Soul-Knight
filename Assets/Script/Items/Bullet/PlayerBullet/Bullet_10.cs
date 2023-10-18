using UnityEngine;

public class Bullet_10 : IPlayerBullet
{
    public Bullet_10(GameObject obj, PlayerWeaponShareAttribute attr) : base(obj, attr)
    {
        type = PlayerBulletType.Bullet_10;
    }
}
